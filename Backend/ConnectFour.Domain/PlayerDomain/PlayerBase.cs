using ConnectFour.Domain.GridDomain;
using ConnectFour.Domain.GridDomain.Contracts;
using ConnectFour.Domain.PlayerDomain.Contracts;

namespace ConnectFour.Domain.PlayerDomain;

/// <inheritdoc cref="IPlayer"/>
public class PlayerBase : IPlayer
{
    public Guid Id { get; }

    public string Name { get; }
/// <inheritdoc/>

    public DiscColor Color { get; }
/// <inheritdoc/>

    public int NumberOfNormalDiscs { get; set; }

    public IReadOnlyList<IDisc> SpecialDiscs { get; set; }

    public PlayerBase(Guid userId, string name, DiscColor color, int numberOfNormalDiscs)
    {
        Id = userId;
        Name = name;
        Color = color;
        NumberOfNormalDiscs = numberOfNormalDiscs;
    }

    /// <summary>
    /// Creates a player that is a copy of an other player.
    /// </summary>
    /// <remarks>
    /// This is an EXTRA. Not needed to implement the minimal requirements.
    /// To make the mini-max algorithm for an AI game play strategy work, this constructor should be implemented.
    /// </remarks>
    public PlayerBase(IPlayer otherPlayer)
    {
        //TODO: copy properties of other player
        //TODO: copy the list of special discs of the other player
        throw new NotImplementedException();
    }

    public bool HasDisk(DiscType discType)
    {
        if (NumberOfNormalDiscs > 0)
        {
            return true;
        }
        else
        { 
            return false;
        }
    }

    public void AddDisc(DiscType discType)
    {
        throw new NotImplementedException();
    }

    public void RemoveDisc(DiscType discType)
    {
        if (NumberOfNormalDiscs <= 0)
        {
            throw new InvalidOperationException();
        }
        else 
        {
            NumberOfNormalDiscs -= 1;
        }
    }
}