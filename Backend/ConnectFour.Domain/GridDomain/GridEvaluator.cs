using ConnectFour.Domain.GridDomain.Contracts;
using System.Drawing;

namespace ConnectFour.Domain.GridDomain;

/// <inheritdoc cref="IGridEvaluator"/>
public class GridEvaluator : IGridEvaluator

{
    public int CalculateScore(IGrid grid, DiscColor maximizingColor)
    {
        if (grid.WinningConnections.Count > 0)
        {
            if (grid.WinningConnections.First().Color == maximizingColor)
            {
                return  int.MaxValue; 
            }

            else 
            {
                return int.MinValue;
            }
            
        }
        else 
        {
            return 1;

        }

          
    }
}