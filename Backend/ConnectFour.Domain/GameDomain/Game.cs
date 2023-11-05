using ConnectFour.Domain.GameDomain.Contracts;
using ConnectFour.Domain.GridDomain;
using ConnectFour.Domain.GridDomain.Contracts;
using ConnectFour.Domain.PlayerDomain.Contracts;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Intrinsics.X86;

namespace ConnectFour.Domain.GameDomain;

/// <inheritdoc cref="IGame"/>
internal class Game : IGame
{
    public Guid Id { get; set; }

    public IPlayer Player1 { get; set; }

    public IPlayer Player2 { get; set; }

    public Guid PlayerToPlayId { get; set; }

    public IGrid Grid { get; set; }

    public bool Finished {
        get
        {
            if (GetPlayerById(PlayerToPlayId).HasDisk(DiscType.Normal) == false || Grid.WinningConnections.Count > 0)
            {
                return true;
            }
            else { return false; }


        }
    }

    public bool PopOutAllowed { get; set; }

    public Game(IPlayer player1, IPlayer player2, IGrid grid, bool popOutAllowed = false)
    {
        Id = Guid.NewGuid();
        Player1 = player1;
        Player2 = player2;
        Grid = grid;
        PopOutAllowed = popOutAllowed;
        PlayerToPlayId = player1.Id;
    }

    /// <summary>
    /// Creates a game that is a copy of an other game.
    /// </summary>
    /// <remarks>
    /// This is an EXTRA. Not needed to implement the minimal requirements.
    /// To make the mini-max algorithm for an AI game play strategy work, this constructor should be implemented.
    /// </remarks>
    public Game(IGame otherGame)
    {
        //TODO: make a copy of the players
        //TODO: make a copy of the grid
        //TODO: initialize the properties with the copies
        throw new NotImplementedException();
    }

    public IReadOnlyList<IMove> GetPossibleMovesFor(Guid playerId)
    {
        IReadOnlyList<IMove> listmoves = new List<IMove>();
        if (PlayerToPlayId != playerId)
        {
            return listmoves;
        }
        else if (GetPlayerById(playerId).HasDisk(DiscType.Normal) == false)
        {
            return listmoves;
        }
        else
        {
            for (int j = 0; j < Grid.NumberOfColumns; j++)
            {
                if (Grid.Cells[0, j] is null)
                {
                    ((List<IMove>)listmoves).Add(new Move(j));
                }
            }
            return listmoves;
        }
    }

    public void ExecuteMove(Guid playerId, IMove move)
    {
        if (Finished)
        {
            throw new InvalidOperationException();
        }
        else if (playerId != PlayerToPlayId)
        {
            throw new InvalidOperationException();
        }

        else if (GetPlayerById(playerId).HasDisk(move.DiscType)==false)
        {
            throw new InvalidOperationException();
        }
        else
        {
            Grid.SlideInDisc(new Disc(DiscType.Normal, GetPlayerById(playerId).Color), move.Column);
            if (PlayerToPlayId == Player1.Id)
            {
                PlayerToPlayId = Player2.Id;
                Player1.RemoveDisc(move.DiscType);
            }
            else
            {
                PlayerToPlayId = Player1.Id;
                Player2.RemoveDisc(move.DiscType);
            }
        }
    }

    public IPlayer GetPlayerById(Guid playerId)
    {
        if (playerId != Player1.Id && playerId != Player2.Id)
        {
            throw new InvalidOperationException();
        }
        else if (playerId == Player1.Id)
        {
            return Player1;
        }
        else
        {
            return Player2;
        }
    }

    public IPlayer GetOpponent(Guid playerId)
    {
        if (playerId != Player1.Id &&playerId != Player2.Id)
        {
            throw new InvalidOperationException();
        }
        else if (playerId == Player1.Id)
        {
            return Player2;
        }
        else
        {
            return Player1;
        }
    }
}
