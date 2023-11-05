using ConnectFour.Domain.GameDomain.Contracts;
using ConnectFour.Domain.GridDomain;

namespace ConnectFour.Domain.GameDomain;

/// <inheritdoc cref="IMove"/>
public class Move : IMove
{
    public Move(int column, MoveType type = MoveType.SlideIn, DiscType discType = DiscType.Normal)
    {
        Column = column;
        Type = type;
        DiscType = discType;
    }

    //public Move(int column)
    //{
    //    Column = column;
    //}

    public int Column { get; }

    public MoveType Type { get; }

    public DiscType DiscType { get; } 

    
}