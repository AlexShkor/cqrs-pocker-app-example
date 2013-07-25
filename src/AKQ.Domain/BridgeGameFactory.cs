using System;
using System.Linq;
using AKQ.Domain.Documents;
using AKQ.Domain.Documents.Progress;
using AKQ.Domain.Messaging;
using AKQ.Domain.Services;
using Microsoft.Practices.ServiceLocation;
using MongoDB.Bson;
using RestSharp.Extensions;

namespace AKQ.Domain
{
    public class BridgeGameFactory
    {
        private const string AttackKey = "Attack";
        private const string DefenceKey = "Defence";

        private readonly BridgeDealService _bridgeDealService;
        private readonly UserProgressService _progressService;
        private readonly IServiceLocator _serviceLocator;
        private readonly TournamentDocumentsService _tournaments;
        private readonly UsersService _usersService;

        public BridgeGameFactory(BridgeDealService bridgeDealService, IServiceLocator serviceLocator, TournamentDocumentsService tournaments, UsersService usersService, UserProgressService progressService)
        {
            _bridgeDealService = bridgeDealService;
            _serviceLocator = serviceLocator;
            _tournaments = tournaments;
            _progressService = progressService;
            _usersService = usersService;
        }

        public BridgeGame RandomAttack(string gameId, string userId, string username)
        {
            return CreateFromDeal(gameId, _bridgeDealService.GetRandomDeal(DealTypeEnum.Attack), userId, username);
        }

        public BridgeGame Attack(string gameId, string dealId, string userId, string username)
        {
            return CreateFromDeal(gameId, GetPracticeDeal(DealTypeEnum.Attack, dealId, userId), userId, username);
        }

        public BridgeGame Defence(string gameId, string dealId, string userId, string username)
        {
            return CreateFromDeal(gameId, GetPracticeDeal(DealTypeEnum.Defence, dealId, userId), userId, username);
        }

        private BridgeGame CreateFromDeal(string gameId, BridgeDeal deal, string userId, string username, Action<BridgeGame> beforeInitialize = null)
        {
            var bid = deal.BestContract.GetValueAndSuit();
            var declarer = PlayerPosition.FromShortName(deal.BestContract.Position);
            var game = new BridgeGame(gameId, _serviceLocator.GetInstance<IBridgeGameCallback>());
            if (beforeInitialize != null)
            {
                beforeInitialize(game);
            }
            game.Initialize(deal.Id,  deal.DealType, userId, username, deal.PBNHand, declarer, PlayerPosition.South);
            game.AddBid(Bid.FromString(bid));
            game.AddBid(Bid.Pass);
            game.AddBid(Bid.Pass);
            game.AddBid(Bid.Pass);
            return game;
        }

        private BridgeDeal GetDeal(DealTypeEnum? gameType,string id)
        {
            return id == null
               ? _bridgeDealService.GetRandomDeal(gameType)
               : _bridgeDealService.GetById(id);
        }

        private BridgeDeal GetPracticeDeal(DealTypeEnum gameType, string dealId, string userId)
        {
            if (!dealId.HasValue())
            {
                var userprogress = _progressService.GetById(userId) ?? new UserProgress() {Id = userId};
                var progress = userprogress.PracticeProgress;
                var gameSet = progress.GetCurrentGameSet();
                const int setsLimit = 20;
                if (gameSet == null && progress.GameSets.Count >= setsLimit)
                {
                    return null;
                }
                if (gameSet == null)
                {
                    var index = 1;
                    var skip = 0;
                    const int count = 5;
                    var last = progress.GetLastGameSet();
                    if (last != null)
                    {
                        index = last.Index + 1;
                        skip = last.Skip + count;
                    }
                    var deals = _bridgeDealService.Get(gameType, skip, count);
                    gameSet = new GameSet(index, skip, count, deals);
                    progress.GameSets.Add(gameSet);
                    _progressService.Save(userprogress);
                }
                dealId = gameSet.GetNextDeal();
            }
            return _bridgeDealService.GetById(dealId);
        }

        public BridgeGame CreateGame(string gameId, GameModeEnum gameMode, GameCreationData data)
        {
            BridgeGame game = null;
            switch (gameMode)
            {
                case GameModeEnum.RandomDeal:
                    game=  RandomAttack(gameId, data.UserId, data.UserName);
                    break;
                case GameModeEnum.PracticeAttackSets:
                    game = PracticeAttackSets(gameId, data.DealId, data.UserId, data.UserName);
                    break;
                case GameModeEnum.PracticeDefenceSets:
                    break;
                case GameModeEnum.Repetition:
                    game = Repetition(gameId, data.UserId, data.UserName);
                    break;
                case GameModeEnum.WatchRobotsPlay:
                    game =  Auto(gameId, data.DealId);
                    break;
                case GameModeEnum.SpecificDeal:
                    game = SpecificDeal(gameId, data.DealId, data.UserId, data.UserName);
                    break;
            }
            if (game == null)
            {
                game = RandomAttack(gameId, data.UserId, data.UserName);
                game.GameMode = GameModeEnum.RandomDeal;
            }
            else
            {
                game.GameMode = gameMode;
            }
            return game;
        }

        private BridgeGame SpecificDeal(string gameId, string dealId, string userId, string userName)
        {
            return CreateFromDeal(gameId, _bridgeDealService.GetById(dealId), userId, userName);
        }

        private BridgeGame Repetition(string gameId, string userId, string userName)
        {
            var userprogress = _progressService.GetById(userId) ?? new UserProgress() {Id = userId};
            var progress = userprogress.RepetitionProgress;
            const int dealsCount = 15;
            if (progress.Deals.Count == 0)
            {
                var deals = _bridgeDealService.Get(DealTypeEnum.Attack, 0, dealsCount);
                progress.Deals.AddRange(deals.Select(x => new DealStats(x)));
                _progressService.Save(userprogress);
            }
            var deal = progress.GetCurrentDeal();
            if (deal == null)
            {
                return null;
            }
            var bridgeDeal = _bridgeDealService.GetById(deal.DealId);
            return CreateFromDeal(gameId, bridgeDeal, userId, userName);
        }

        private BridgeGame PracticeAttackSets(string gameId, string dealId, string userId, string userName)
        {
            var deal = GetPracticeDeal(DealTypeEnum.Attack, dealId, userId);
            if (deal == null)
            {
                return null;
            }
            return CreateFromDeal(gameId, deal, userId, userName);
        }

        public BridgeGame FromDeal(string gameId, string gameType, string dealId, string userId, string userName, string tournamentId)
        {
            if (tournamentId.HasValue())
            {
                var tournament = _tournaments.GetById(tournamentId);
                dealId = tournament.GetNextDealIdFor(userId);
                var isLast = dealId == tournament.Deals.Last();
                var tableNumber = tournament.Deals.IndexOf(dealId) + 1;
                return Tournament(tournamentId, gameId, userId, userName, tournament.HandsToPlay, tournament.MinutesToPlay, dealId, tournament.StartTime, isLast, tableNumber);
            }
            if (dealId.HasValue())
            {
                return CreateFromDeal(gameId, GetDeal(null, dealId), userId, userName);
            }
            switch (gameType.ToLower())
            {
                case "random":
                    return RandomAttack(gameId, userId, userName);
                case "attack":
                    return Attack(gameId, dealId, userId, userName);  
                case "defence":
                    return Defence(gameId, dealId, userId, userName);     
                case "auto":
                    return Auto(gameId, dealId);
                case "tournament":
                    return Tournament(ObjectId.GenerateNewId().ToString(),gameId, userId, userName,3,10);
                default:
                    throw new ArgumentOutOfRangeException(gameType);
            }
        }

        private BridgeGame Tournament(string tournamentId,  string gameId, string userId, string userName, int handsToPlay, int minutesToPlay, 
            string dealId = null, 
            DateTime? startedDate = null, 
            bool isLast = false, 
            int tableNumber = 1)
        {
            var deal = GetDeal(DealTypeEnum.Attack, dealId);
            return CreateFromDeal(gameId, deal, userId, userName, bridgeGame => bridgeGame.Tournament(tournamentId, handsToPlay, minutesToPlay, startedDate, isLast, tableNumber));
        }

        private BridgeGame Auto(string gameId, string dealId)
        {
            return CreateFromDeal(gameId, GetDeal(DealTypeEnum.Attack, dealId), null, null, game => game.UseDDS = true);
        }
    }

    public class GameCreationData
    {
        public GameCreationData(string dealId, string userId, string userName, string tournamentId)
        {
            DealId = dealId;
            UserId = userId;
            UserName = userName;
            TournamentId = tournamentId;
        }

        public string UserId { get; set; }
        public string UserName { get; set; }
        public string DealId { get; set; }
        public string TournamentId { get; set; }
    }

    public enum GameModeEnum
    {
        RandomDeal = 0,
        PracticeAttackSets = 1,
        PracticeDefenceSets = 2,
        Repetition = 3,
        WatchRobotsPlay = 4,
        Tournament = 5,
        SpecificDeal = 6
    }
}