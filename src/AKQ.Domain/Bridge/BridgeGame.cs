using System;
using System.Collections.Generic;
using System.Linq;
using AKQ.Domain.Documents;
using AKQ.Domain.Messaging;
using AKQ.Domain.PBN;
using AKQ.Domain.Utils;

namespace AKQ.Domain
{
    [Serializable]
    public class BridgeGame
    {
        [NonSerialized]
        readonly IBridgeGameCallback _callback;
        readonly Deck _originalHands;
        readonly Deck _hands;

        public BridgeGame(string gameId, IBridgeGameCallback bridgeGameCallback)
        {
            Bids = new List<PlayerBid>();
            IsBidding = true;
            CompletedTricks = new List<Trick>();
            Id = gameId;
            PlayersManager = new PlayersManager();
            _callback = bridgeGameCallback;
            _callback.SetGameId(Id);
            _hands = new Deck();
            _originalHands = new Deck();
        }

        public void Initialize(string dealId, DealTypeEnum gameType, string userId, string username, string pbnHand, PlayerPosition dealer, PlayerPosition user)
        {
            DealId = dealId;
            GameType = gameType;
            PlayersManager.Get(user).TakePlace(userId,username);
            PlayersManager.SetFirstPlayer(dealer);
            SetHands(pbnHand);
            Created = DateTime.Now;
            HostUserId = userId;
            _callback.GameCreated(userId, gameType,DealId);
        }

        public void Tournament(string tournamentId, int handsToPlay, int minutesToPlay, DateTime? startedDate, bool isLast, int tableNumber)
        {
            TournamentInfo = new TournamentInfo
            {
                Id = tournamentId,
                HandsToPlay = handsToPlay,
                MinutesToPlay = minutesToPlay,
                TournamentStarted = startedDate,
                IsLastGame = isLast,
                TableNumber = tableNumber
            };
        }

        public void Load(BridgeGameDocument doc)
        {
            Id = doc.Id;
            Created = doc.Created;
            Started = doc.Started;
            HostUserId = doc.HostUserId;
            DealId = doc.DealId;
            _originalHands.ParseFromPBN(doc.OridinalHandsPBN);
            _callback.Disabled = true;
            _hands.ParseFromPBN(doc.OridinalHandsPBN);
            Contract = doc.Contract.ToDomainObject();
        }

        public string Id { get; private set; }
        public DateTime Created { get; private set; }
        public DateTime? Started { get; private set; }
        public string DealId { get; private set; }
        public PlayersManager PlayersManager { get; private set; }
        public Deck OriginalHands { get { return _originalHands; } }
        public Deck Hands { get { return _hands; } }
        public List<Trick> CompletedTricks { get; private set; }
        public Trick CurrentTrick { get; private set; }
        public bool IsBidding { get; private set; }
        public List<PlayerBid> Bids { get; private set; }
        public Contract Contract { get; private set; }
        public TournamentInfo TournamentInfo { get; private set; }
        public DealTypeEnum GameType { get; private set; }
        public GameModeEnum GameMode { get; set; }
        public Suit TrumpSuit { get { return Contract.Suit; }}

        protected string HostUserId { get; private set; }

        public bool IsFinished
        {
            get { return CompletedTricks.Count == 13; }
        }
        public bool IsStarted
        {
            get { return Started.HasValue; }
        }

        public int EwTricksCount
        {
            get { return CompletedTricks.Count(x => x.Winner.IsEastWestTeam); }
        }

        public int NsTricksCount
        {
            get { return CompletedTricks.Count(x => x.Winner.IsNorthSouthTeam); }
        }

        public int Result
        {
            get { return TricksWonCount - Contract.GetTarget(); }
        }

        public int TricksWonCount
        {
            get { return CompletedTricks.Count(x => x.Winner.IsInTeamWith(Contract.Declarer)); }
        }

        private void SetHands(string pbn)
        {
            _originalHands.ParseFromPBN(pbn);
            _hands.ParseFromPBN(pbn);
        }

        public void AddBid(Bid bid)
        {
            Bids.Add(new PlayerBid(PlayersManager.CurrentPosition, bid));
            PlayersManager.NextPlayer();
            if (AreLastThreeBidsPassed())
            {
                IsBidding = false;
                var last = Bids.Last(x => x.Bid.HasValue);
                Contract = new Contract(last.Bid.Value, last.Bid.Suit, last.Position);
                CurrentTrick = new Trick(TrumpSuit);
                PlayersManager.SetDeclarer(Contract.Declarer);
            }
            else
            {
                IsBidding = true;
            }
        }

        private bool AreLastThreeBidsPassed()
        {
            return Bids.Count > 3 && Bids.Skip(Bids.Count - 3).Take(3).All(x => x.Bid == Bid.Pass);
        }

        public void Start(string userId)
        {
            if (Started.HasValue)
            {
                throw new InvalidOperationException("Game already started");
            }
            if (userId != HostUserId)
            {
                //throw
                return;
            }
            Started = DateTime.Now;
            if (TournamentInfo != null )
            {
                if (TournamentInfo.TournamentStarted == null)
                {
                    TournamentInfo.TournamentStarted = Started;
                }
                TournamentInfo.GameStarted = Started;
            }
            _callback.GameStarted(Contract, TournamentInfo, HostUserId);
            SendPlayerTurnCallback(false);
        }

        public void Join(string userId,string username, PlayerPosition position)
        {
            if (PlayersManager.CanJoin(userId))
            {
                PlayersManager.TakePlace(position, userId, username);
                _callback.PlayerJoined(position, userId, username);
            }
        }

        public void PlayCard(PlayerPosition player, Card card)
        {
            if (player != PlayersManager.CurrentPosition)
            {
                throw new InvalidOperationException(string.Format("Player {0} trying to play card, but here is {1} turn", player, PlayersManager.CurrentPlayer));
            }
            if (!Hands[player].HasCard(card))
            {
                throw new InvalidOperationException(string.Format("Card not foung {0} {1}", player, card));
            }
            Hands[player].Remove(card);
            CurrentTrick.AddCard(player, card);
            var trickIsComplete = CurrentTrick.TrickIsComplete;
            _callback.CardPlayed(card, player, trickIsComplete);
            if (trickIsComplete)
            {
                CompleteTrick();
            }
            else
            {
                PlayersManager.NextPlayer();
            }
            if (IsFinished)
            {
                _callback.GameFinished(OriginalHands, this.GetPBN(), this.GetPBNForRoboBridge(), Result, TournamentInfo,DateTime.Now, HostUserId, DealId);
            }
            else
            {
                SendPlayerTurnCallback(trickIsComplete);
            }
        }

        private void CompleteTrick()
        {
            CompletedTricks.Add(CurrentTrick);
            PlayersManager.CurrentPlayer = IsFinished ? null : PlayersManager.Get(CurrentTrick.Winner);
            _callback.TrickEnded(CurrentTrick.Winner,CompletedTricks.Count, CurrentTrick);
            CurrentTrick = new Trick(TrumpSuit);
        }

        private void SendPlayerTurnCallback(bool isNextTrick)
        {
            _callback.PlayerTurnStarted(PlayersManager.CurrentPlayer.PlayerPosition, CurrentTrick.LedSuit,
                                        PlayersManager.CurrentPlayer.IsAI,
                                        PlayersManager.CurrentOrPartner.PlayerPosition, isNextTrick, this.GetPBNForRoboBridge(), PlayersManager.CurrentOrPartner.UserId, TrickNumber, UseDDS, this.GetPBN());
        }

        public bool UseDDS { get; set; }

        protected int TrickNumber
        {
            get { return CompletedTricks.Count + 1; }
        }

        public bool IsHostedBy(string userId)
        {
            return HostUserId == userId;
        }
    }
}