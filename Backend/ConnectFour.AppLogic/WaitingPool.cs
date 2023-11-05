using ConnectFour.AppLogic.Contracts;
using ConnectFour.Common;
using ConnectFour.Domain;
using ConnectFour.Domain.GameDomain;
using ConnectFour.Domain.GameDomain.Contracts;

namespace ConnectFour.AppLogic;

/// <inheritdoc cref="IWaitingPool"/>
internal class WaitingPool : IWaitingPool
{
    public IGameCandidateFactory GameCandidateFactory { get; set; }
    public IGameCandidateRepository GameCandidateRepository { get; set; }
    public IGameCandidateMatcher GameCandidateMatcher { get; set; }
    public IGameService GameService { get; set; }

    public WaitingPool(
        IGameCandidateFactory gameCandidateFactory,
        IGameCandidateRepository gameCandidateRepository,
        IGameCandidateMatcher gameCandidateMatcher,
        IGameService gameService)
    {
        GameCandidateFactory = gameCandidateFactory;
        GameCandidateRepository = gameCandidateRepository;
        GameCandidateMatcher = gameCandidateMatcher;
        GameService = gameService;
    }

    public void Join(User user, GameSettings gameSettings)
    {
        IGameCandidate candidate = GameCandidateFactory.CreateNewForUser(user, gameSettings);
        GameCandidateRepository.AddOrReplace(candidate);
        if (gameSettings.AutoMatchCandidates == true)
        {
            IList<IGameCandidate> possibleCandidates = FindCandidatesThatCanBeChallengedBy(user.Id);
            IGameCandidate opponent = GameCandidateMatcher.SelectOpponentToChallenge(possibleCandidates);

            if (opponent is not null)
            {
                candidate.Challenge(opponent);
                opponent.AcceptChallenge(candidate);

                // Create game
                IGame game = GameService.CreateGameForUsers(user, opponent.User, gameSettings);
                candidate.GameId = game.Id;
                opponent.GameId = game.Id;
            }
        }
    }

    public void Leave(Guid userId)
    {
        GameCandidateRepository.RemoveCandidate(userId);
    }

    public IGameCandidate GetCandidate(Guid userId)
    {
        return GameCandidateRepository.GetCandidate(userId);
    }

    public void Challenge(Guid challengerUserId, Guid targetUserId)
    {
        //IGameCandidate challenger = GetCandidate(challengerUserId);
        //IGameCandidate target = GetCandidate(targetUserId);
        
        //if (challenger is null)
        //{
        //    throw new ArgumentException($"Invalid challenger id: {challengerUserId}");
        //}

        //if (target is null)
        //{
        //    throw new ArgumentException($"Invalid target id: {targetUserId}");
        //}

        //challenger.Challenge(target);
    }


    public IList<IGameCandidate> FindCandidatesThatCanBeChallengedBy(Guid userId)
    {
        return GameCandidateRepository.FindCandidatesThatCanBeChallengedBy(userId);        
    }

    public void WithdrawChallenge(Guid userId)
    {
        throw new NotImplementedException();
    }

    public IList<IGameCandidate> FindChallengesFor(Guid challengedUserId)
    {
        return GameCandidateRepository.FindChallengesFor(challengedUserId);
    }
}