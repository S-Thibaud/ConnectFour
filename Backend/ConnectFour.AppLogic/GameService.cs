using ConnectFour.AppLogic.Contracts;
using ConnectFour.Domain;
using ConnectFour.Domain.GameDomain;
using ConnectFour.Domain.GameDomain.Contracts;
using System.Runtime.Intrinsics.X86;

namespace ConnectFour.AppLogic;

/// <inheritdoc cref="IGameService"/>
internal class GameService : IGameService
{
    public GameService(
        IGameFactory gameFactory,
        IGameRepository gameRepository)
    {
        GameFactory = gameFactory;
        GameRepository = gameRepository;
    }

    public IGameFactory GameFactory { get; }
    public IGameRepository GameRepository { get; }

    // TODO
    public IGame CreateGameForUsers(User user1, User user2, GameSettings settings)
    {
        IGame game = GameFactory.CreateNewTwoPlayerGame(settings, user1, user2);
        GameRepository.Add(game);
        return game;
    }

    public IGame GetById(Guid gameId)
    {
        return GameRepository.GetById(gameId);
    }

    public void ExecuteMove(Guid gameId, Guid playerId, IMove move)
    {
        GameRepository.GetById(gameId).ExecuteMove(playerId, move);
    }

    public IGame CreateSinglePlayerGameForUser(User user, GameSettings settings)
    {
        IGame game = GameFactory.CreateNewSinglePlayerGame(settings, user);
        GameRepository.Add(game);
        return game;
    }
}