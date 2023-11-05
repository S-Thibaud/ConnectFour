using ConnectFour.Domain.GridDomain.Contracts;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Drawing;

namespace ConnectFour.Domain.GridDomain;

/// <inheritdoc cref="IConnection"/>
public class Connection : IConnection
{
    public static Connection Empty
    {
        get
        {
            return new Connection();
        }
    }

    public GridCoordinate From { get; }

    public GridCoordinate To { get; }

    public int Size { get; }

    public DiscColor Color { get; }

    public Connection(int rowFrom, int columnFrom, int rowTo, int columnTo, DiscColor color)
    {
        From = new GridCoordinate(rowFrom, columnFrom);
        To = new GridCoordinate(rowTo, columnTo);
        Color = color;
        if (rowFrom == rowTo)
        {
            Size = columnTo - columnFrom + 1;
        }
        else
        {
            if (rowTo > rowFrom)
            {
                Size = rowTo - rowFrom + 1;
            }
            else
            {
                Size = rowFrom - rowTo + 1;
            }
        }
    }

    private Connection() 
    {
        Color = DiscColor.Red;
        From = GridCoordinate.Empty;
        To = GridCoordinate.Empty;
    }
}