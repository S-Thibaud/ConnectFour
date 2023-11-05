using ConnectFour.Domain.GameDomain.Contracts;
using ConnectFour.Domain.GridDomain;
using ConnectFour.Domain.PlayerDomain;
using ConnectFour.Domain.PlayerDomain.Contracts;
using System.Runtime.Intrinsics.X86;

namespace ConnectFour.Domain.GameDomain;

/// <inheritdoc cref="IGameFactory"/>
internal class GameFactory : IGameFactory
{
    // EXTRA
    public GameFactory(IGamePlayStrategy gamePlayStrategy)
    {
        GamePlayStrategy = gamePlayStrategy;
    }

    IGamePlayStrategy GamePlayStrategy { get; set; }

    // EXTRA
    public IGame CreateNewSinglePlayerGame(GameSettings settings, User user)
    {
        Grid _grid = new Grid(settings);
        // game aanmaken
        // grid maken + expected disks maken
        HumanPlayer player = new HumanPlayer(user.Id, user.NickName, DiscColor.Red, _grid.NumberOfRows * _grid.NumberOfColumns / 2);
        return new Game(player, null, new Grid(), false);
    }

    // TODO
    public IGame CreateNewTwoPlayerGame(GameSettings settings, User user1, User user2)
    {
        if (user1.Id == user2.Id)
        {
            throw new InvalidOperationException();
        }
        else
        {
            Grid _grid = new Grid(settings);
            // game aanmaken
            // grid maken + expected disks maken
            HumanPlayer player1 = new HumanPlayer(user1.Id, user1.NickName, DiscColor.Red, _grid.NumberOfRows * _grid.NumberOfColumns / 2);
            HumanPlayer player2 = new HumanPlayer(user2.Id, user2.NickName, DiscColor.Yellow, _grid.NumberOfRows * _grid.NumberOfColumns / 2);
            return new Game(player1, player2, _grid, false);
        }
    }
}