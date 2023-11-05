using ConnectFour.Domain.GameDomain;
using ConnectFour.Domain.GridDomain.Contracts;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ConnectFour.Domain.GridDomain;

/// <inheritdoc cref="IGrid"/>
public class Grid : IGrid
{
    public Grid(GameSettings settings = null)
    {
        settings = settings ?? new GameSettings();
        NumberOfRows = settings.GridRows;
        NumberOfColumns = settings.GridColumns;
        WinningConnectSize = settings.ConnectionSize;
        WinningConnections = new List<Connection>();
        Cells = new Disc[NumberOfRows,NumberOfColumns];
    }

    /// <summary>
    /// Creates a grid that is a copy of an other grid.
    /// </summary>
    /// <remarks>
    /// This is an EXTRA. Not needed to implement the minimal requirements.
    /// To make the mini-max algorithm for an AI game play strategy work, this constructor should be implemented.
    /// </remarks>
    public Grid(IGrid otherGrid)
    {
        //TODO: create a cells matrix and copy the values from the other grid
        //TODO: copy other property values
        //for (int row = 0; row < Cells.GetLength(0); row++)
        //{
        //    for (int collum = 0; collum < Cells.GetLength(1); collum++) 
        //    {
        //        Cells[row, collum] = otherGrid.Cells[row, collum];
        //    }
        //}
        Cells = otherGrid.Cells;
        NumberOfRows = otherGrid.NumberOfRows;
        NumberOfColumns = otherGrid.NumberOfColumns;
        WinningConnectSize = otherGrid.WinningConnectSize;
        WinningConnections = new List<Connection>();
    }

    public int NumberOfRows { get; }

    public int NumberOfColumns { get; }

    public int WinningConnectSize { get; }

    public IDisc[,] Cells { get; }
/// <inheritdoc/>

    public IReadOnlyList<IConnection> WinningConnections { get; }

    public void PopOutDisc(IDisc disc, int column)
    {
        throw new NotImplementedException();
    }

    public void SlideInDisc(IDisc disc, int column)
    {
        // de disc naar beneden laten vallen
        for (int row = 0; row < NumberOfRows; row++)
        {
            if (row + 1 == NumberOfRows || Cells[row + 1, column] is not null)
            {
                if (Cells[0, column] is not null)
                {
                    throw new InvalidOperationException();
                }
                else
                {
                    Cells.SetValue(disc, row, column);

                    //if (winnendeVerbinding(row, column, disc) is not null)
                    //{
                    //    // als er een winnende verbinding is maak een nieuwe verbinding aan en voeg hem toe aan de lijst
                    //    Connection winnend = winnendeVerbinding(row, column, disc);
                    //    ((List<Connection>)WinningConnections).Add(winnend);
                    //}
                    winnendeVerbinding(row, column, disc);

                    //row = NumberOfRows + 1;
                }
                row = NumberOfRows + 1;
            }

        }
    }

    private void winnendeVerbinding(int row, int col, IDisc disc)
    {
        // variabelen
        int rowlast = NumberOfRows - 1;
        int collast = NumberOfColumns - 1;
        int rowfirst = 0;
        int colfirst = 0;
        int count = 1;
        int check;
        List<Connection> connections = new List<Connection>();

        // SCHUINE VERBINDINGEN

        // RECHTS SCHUIN BENEDEN
        for (check = 1; check < WinningConnectSize; check++)
        {
            // controleren of we niet uit de tabel zijn
            if (row - check >= 0 && col + check < NumberOfColumns)
            {
                // controleren of de plaats niet leeg is
                if (Cells[row - check, col + check] is not null)
                {
                    if (Cells[row, col].Color.Equals(Cells[row - check, col + check].Color))
                    {
                        // de verste coordinaten meegeven
                        rowlast = row - check;
                        collast = col + check;

                        count++;
                    }
                    else
                    {
                        // de verste coordinaten meegeven
                        rowlast = row - check + 1;
                        collast = col + check - 1;

                        check = WinningConnectSize + 1;
                    }
                }
                else
                {
                    // de verste coordinaten meegeven
                    rowlast = row - check + 1;
                    collast = col + check - 1;

                    check = WinningConnectSize + 1;
                }
            }
            else
            {
                // de verste coordinaten meegeven
                rowlast = row - check + 1;
                collast = col + check - 1;

                check = WinningConnectSize + 1;
            }
        }

        // RECHTS SCHUIN BOVEN
        for (check = 1; check < WinningConnectSize; check++)
        {
            // controleren of we niet uit de tabel zijn
            if (row + check < NumberOfRows && col - check >= 0)
            {
                // controleren of de plaats niet leeg is
                if (Cells[row + check, col - check] is not null)
                {
                    if (Cells[row, col].Color.Equals(Cells[row + check, col - check].Color))
                    {
                        // de verste coordinaten meegeven
                        rowfirst = row + check;
                        colfirst = col - check;

                        count++;
                    }
                    else
                    {
                        // de verste coordinaten meegeven
                        rowfirst = row + check - 1;
                        colfirst = col - check + 1;

                        check = WinningConnectSize + 1;
                    }
                }
                else
                {
                    // de verste coordinaten meegeven
                    rowfirst = row + check - 1;
                    colfirst = col - check + 1;

                    check = WinningConnectSize + 1;
                }
            }
            else
            {
                // de verste coordinaten meegeven
                rowfirst = row + check - 1;
                colfirst = col - check + 1;

                check = WinningConnectSize + 1;
            }
        }
        if (count == WinningConnectSize)
        {
            // als er een winnende verbindingen zijn returnt een false 
            connections.Add(new Connection(rowfirst, colfirst, rowlast, collast, disc.Color));
        }
        count = 1;
        //else
        //{
        //    count = 1;
        //}

        // LINKS SCHUIN BENEDEN
        for (check = 1; check < WinningConnectSize; check++)
        {
            // controleren of we niet uit de tabel zijn
            if (row - check >= 0 && col - check >= 0)
            {
                // controleren of de plaats niet leeg is
                if (Cells[row - check, col - check] is not null)
                {
                    if (Cells[row, col].Color.Equals(Cells[row - check, col - check].Color))
                    {
                        // de verste coordinaten meegeven
                        rowfirst = row - check;
                        colfirst = col - check;

                        count++;
                    }
                    else
                    {
                        // de verste coordinaten meegeven
                        rowfirst = row - check + 1;
                        colfirst = col - check + 1;

                        check = WinningConnectSize + 1;
                    }
                }
                else
                {
                    // de verste coordinaten meegeven
                    rowfirst = row - check + 1;
                    colfirst = col - check + 1;

                    check = WinningConnectSize + 1;
                }
            }
            else
            {
                // de verste coordinaten meegeven
                rowfirst = row - check + 1;
                colfirst = col - check + 1;

                check = WinningConnectSize + 1;
            }
        }

        // LINKS SCHUIN BOVEN
        for (check = 1; check < WinningConnectSize; check++)
        {
            // controleren of we niet uit de tabel zijn
            if (row + check < NumberOfRows && col + check < NumberOfColumns)
            {
                // controleren of de plaats niet leeg is
                if (Cells[row + check, col + check] is not null)
                {
                    if (Cells[row, col].Color.Equals(Cells[row + check, col + check].Color))
                    {
                        // de verste coordinaten meegeven
                        rowlast = row + check;
                        collast = col + check;

                        count++;
                    }
                    else
                    {
                        // de verste coordinaten meegeven
                        rowlast = row + check - 1;
                        collast = col + check - 1;

                        check = WinningConnectSize + 1;
                    }
                }
                else
                {
                    // de verste coordinaten meegeven
                    rowlast = row + check - 1;
                    collast = col + check - 1;

                    check = WinningConnectSize + 1;
                }
            }
            else
            {
                // de verste coordinaten meegeven
                rowlast = row + check - 1;
                collast = col + check - 1;

                check = WinningConnectSize + 1;
            }
        }
        if (count == WinningConnectSize)
        {
            // als er een winnende verbindingen zijn returnt een false 
            connections.Add(new Connection(rowfirst, colfirst, rowlast, collast, disc.Color));
        }
        count = 1;
        //else
        //{
        //    count = 1;
        //}

        // HORIZONTAAL RECHTS
        rowfirst = row;
        rowlast = row;

        for (check = 1; check < WinningConnectSize; check++)
        {
            // controleren of we niet uit de tabel zijn
            if (col + check < NumberOfColumns)
            {
                // controleren of de plaats niet leeg is
                if (Cells[row, col + check] is not null)
                {
                    if (Cells[row, col].Color.Equals(Cells[row, col + check].Color))
                    {
                        // de verste coordinaten meegeven
                        collast = col + check;

                        count++;
                    }
                    else
                    {
                        // de verste coordinaten meegeven
                        collast = col + check - 1;

                        check = WinningConnectSize + 1;
                    }
                }
                else
                {
                    // de verste coordinaten meegeven
                    collast = col + check - 1;

                    check = WinningConnectSize + 1;
                }
            }
            else
            {
                // de verste coordinaten meegeven
                collast = col + check - 1;

                check = WinningConnectSize + 1;
            }
        }

        // HORIZONTAAL LINKS
        for (check = 1; check < WinningConnectSize; check++)
        {
            // controleren of we niet uit de tabel zijn
            if (col - check >= 0)
            {
                // controleren of de plaats niet leeg is
                if (Cells[row, col - check] is not null)
                {
                    // yellow 2
                    if (Cells[row, col].Color.Equals(Cells[row, col - check].Color))
                    {
                        // de verste coordinaten meegeven
                        colfirst = col - check;

                        count++;
                    }
                    else
                    {
                        // de verste coordinaten meegeven
                        colfirst = col - check + 1;

                        check = WinningConnectSize + 1;
                    }
                }
                else
                {
                    // de verste coordinaten meegeven
                    colfirst = col - check + 1;

                    check = WinningConnectSize + 1;
                }
            }
            else
            {
                // de verste coordinaten meegeven
                colfirst = col - check + 1;

                check = WinningConnectSize + 1;
            }
        }
        if (count == WinningConnectSize)
        {
            // als er een winnende verbindingen zijn returnt een false 
            connections.Add(new Connection(rowfirst, colfirst, rowlast, collast, disc.Color));
        }
        count = 1;
        //else
        //{
        //    count = 1;
        //}

        // VERTICAAL
        // omdat de kollom niet verandert
        collast = col;
        colfirst = col;

        // VERTICAAL BOVEN
        for (check = 1; check < WinningConnectSize; check++)
        {
            // controleren of we niet uit de tabel zijn
            if (row + check < NumberOfRows)
            {
                // controleren of de plaats niet leeg is
                if (Cells[row + check, col] is not null)
                {
                    if (Cells[row, col].Color.Equals(Cells[row + check, col].Color))
                    {
                        // de verste coordinaten meegeven
                        rowlast = row + check;

                        count++;
                    }
                    else
                    {
                        // de verste coordinaten meegeven
                        rowlast = row + check - 1;

                        check = WinningConnectSize + 1;
                    }
                }
                else
                {
                    // de verste coordinaten meegeven
                    rowlast = row + check - 1;

                    check = WinningConnectSize + 1;
                }
            }
            else
            {
                // de verste coordinaten meegeven
                rowlast = row + check - 1;

                check = WinningConnectSize + 1;
            }
        }

        // VERTICAAL BENEDEN
        for (check = 1; check < WinningConnectSize; check++)
        {
            // controleren of we niet uit de tabel zijn
            if (row - check >= 0)
            {
                // controleren of de plaats niet leeg is
                if (Cells[row - check, col] is not null)
                {
                    if (Cells[row, col].Color.Equals(Cells[row - check, col].Color))
                    {
                        rowfirst = row - check;

                        count++;
                    }
                    else
                    {
                        rowfirst = row - check + 1;

                        check = WinningConnectSize + 1;
                    }
                }
                else
                {
                    rowfirst = row - check + 1;

                    check = WinningConnectSize + 1;
                }
            }
            else
            {
                rowfirst = row - check + 1;

                check = WinningConnectSize + 1;
            }
        }
        if (count == WinningConnectSize)
        {
            // als er een winnende verbindingen zijn returnt een false 
            connections.Add(new Connection(rowfirst, colfirst, rowlast, collast, disc.Color));
        }

        // als er geen winnende verbindingen zijn returnt een false 
        //return null;

        foreach (Connection conn in connections)
        {
            ((List<Connection>)WinningConnections).Add(conn);
        }
    }
}