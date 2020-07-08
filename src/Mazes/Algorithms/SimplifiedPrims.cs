using Mazes.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Mazes.Algorithms
{
    public class SimplifiedPrims : IAlgorithm
    {
        public IGrid On(IGrid grid)
        {
            return On(grid, grid.RandomCell());
        }

        public IGrid On(IGrid grid, Cell startAt)
        {
            var active = new List<Cell>();
            active.Add(startAt);

            while (active.Any())
            {
                var cell = active.Sample();
                var availableNeighbors = cell.Neighbors().Where(n => !n.Links.Any());

                if (availableNeighbors.Any())
                {
                    var neighbor = availableNeighbors.Sample();
                    cell.Link(neighbor);
                    active.Add(neighbor);
                }
                else
                {
                    active.Remove(cell);
                }
            }

            return grid;
        }
    }
}
