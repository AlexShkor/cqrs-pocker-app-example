using System;
using System.Collections.Generic;
using System.Linq;

namespace AKQ.Domain.ViewModel
{
    public class GameViewModel
    {
        public string GameId { get; set; }
        public string DealId { get; set; }

        public HandViewModel WestHand { get; set; }
        public HandViewModel NorthHand { get; set; }
        public HandViewModel EastHand { get; set; }
        public HandViewModel SouthHand { get; set; }
        public string CurrentPlayer { get; set; }
        public bool IsFinished { get; set; }
        public bool IsStarted { get; set; }
        public TrickViewModel PreviousTrick { get; set; }
        public TrickViewModel CurrentTrick { get; set; }
        public ContractViewModel Contract { get; set; }
        public bool IsBidding { get; set; }
        public bool IsHost { get; set; }
        public EstimatesViewModel Estimates { get; set; }
        public List<PlayerViewModel> JoinedPlayers { get; set; }
        public string TrumpSuit { get; set; }
        public int NsTrickCount { get; set; }
        public int EwTrickCount { get; set; }
        public bool IsLastTournamentTable { get; set; }
        public bool TournamentStarted { get; set; }
        public int SecondsToPlay { get; set; }
        public string NextDealLink { get; set; }
        public string NextDealText { get; set; }
        public bool IsAuth { get; set; }
        public bool IsTournament { get; set; }
        public int TableNumber { get; set; }


        public string ContinueLink { get; set; }
        public TacticsViewModel Tactics { get; set; }

        public GameViewModel(BridgeGame game, string currentUserId)
        {
            GameId = game.Id;
            DealId = game.DealId;
            //var positionsMapping = new Dictionary<PlayerPositionEnum, String>();
            //var position = game.PlayersManager.GetAll().First(x => x.UserId == currentUserId).PlayerPosition;
            //positionsMapping.Add(position,"Botton");
            //positionsMapping.Add(position = PlayersManager.GetNextPlayer(position),"Left");
            //positionsMapping.Add(position = PlayersManager.GetNextPlayer(position), "Top");
            //positionsMapping.Add(PlayersManager.GetNextPlayer(position), "Right");

            var hasHumanPlayer = game.PlayersManager.GetAll().Any(x => !x.IsAI);

            Func<PlayerPosition, HandViewModel> createHandViewModel = (pos) =>
            {
                var player = game.PlayersManager.Get(pos);
                var hasControl = player.HasControl(currentUserId);
                var visible = hasControl || player.IsDummy || !hasHumanPlayer;
                var model = new HandViewModel(
                    game.Hands[pos],
                    visible,
                    game.PlayersManager.CurrentPosition == pos,
                    game.CurrentTrick.LedSuit,
                    game.TrumpSuit);
                model.HasControl = hasControl;
                if (model.HasControl)
                {
                    model.PlayerName = player.Name;
                }
                return model;
            };

            WestHand = createHandViewModel(PlayerPosition.West);
            NorthHand = createHandViewModel(PlayerPosition.North);
            EastHand = createHandViewModel(PlayerPosition.East);
            SouthHand = createHandViewModel(PlayerPosition.South);

            var notVisiblePositions = game.PlayersManager.NotVisiblePositionsFor(currentUserId).ToList();
            Estimates = new EstimatesViewModel(game.OriginalHands, notVisiblePositions[0], notVisiblePositions[1]);

            IsBidding = game.IsBidding;
            IsFinished = game.IsFinished;
            IsStarted = game.IsStarted;
            IsHost = game.IsHostedBy(currentUserId);
            Contract = new ContractViewModel(game.Contract);

            CurrentTrick = new TrickViewModel(game.CurrentTrick);
            PreviousTrick = game.CompletedTricks.LastOrDefault() != null
                                           ? new TrickViewModel(game.CompletedTricks.LastOrDefault())
                                           : null;
            #region Tournament
            if (game.TournamentInfo != null)
            {
                if (game.TournamentInfo.IsLastGame)
                {

                    NextDealLink = string.Format("#/replay/tournament/{0}/game/{1}", game.TournamentInfo.Id, game.Id);
                    NextDealText = "See Tournament Results";
                }
                else
                {
                    NextDealLink = string.Format("#/game/tournament/{0}/next", game.TournamentInfo.Id);
                    NextDealText = "Continue Tournament";
                }
                IsTournament = true;
                SecondsToPlay =(int)game.TournamentInfo.GetTimeToPlay().TotalSeconds;
                TableNumber = game.TournamentInfo.TableNumber;
                TournamentStarted = game.TournamentInfo.TournamentStarted.HasValue;
                IsLastTournamentTable = game.TournamentInfo.IsLastGame;
            }
            else
            {
                NextDealText = "New Deal";
                NextDealLink = "#/game/next/" + game.GameType.ToString() + "/" + game.DealId;
                ContinueLink = "#/game/next/" + game.GameType.ToString();
            }
            if (game.GameMode == GameModeEnum.Repetition)
            {
                NextDealLink = "#/game/create/repetition";
            }
            #endregion

            EwTrickCount = game.EwTricksCount;
            NsTrickCount = game.NsTricksCount;
            CurrentPlayer = game.PlayersManager.CurrentPlayer.ToString();
            TrumpSuit = game.TrumpSuit.ToShortName();
            JoinedPlayers = game.PlayersManager.GetHumanPlayers().Select(x => new PlayerViewModel(x)).ToList();
        }
    }

    public class GameFinishedView
    {
        
    }

    public class PracticeGameFinishedView : GameFinishedView
    {
        
    }

    public class TournamentGameFinishedView : GameFinishedView
    {
        
    }

    public class RepetitionGameFinishedView : GameFinishedView
    {
        
    }
}