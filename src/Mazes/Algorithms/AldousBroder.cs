using Mazes.Extensions;
using System.Linq;

namespace Mazes.Algorithms
{
    public class AldousBroder : IAlgorithm
    {
        public IGrid On(IGrid grid)
        {
            var cell = grid.RandomCell();
            var unvisited = grid.Size - 1;

            while (unvisited > 0)
            {
                var neighbor = cell.Neighbors().Sample();

                if (!neighbor.Links.Any())
                {
                    cell.Link(neighbor);
                    unvisited--;
                }

                cell = neighbor;
            }

            return grid;
        }
    }
}
