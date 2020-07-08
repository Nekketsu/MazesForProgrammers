using Mazes.Extensions;
using System.Linq;

namespace Mazes.Algorithms
{
    public class HuntAndKill : IAlgorithm
    {
        public IGrid On(IGrid grid)
        {
            var current = grid.RandomCell();

            while (current != null)
            {
                var unvisitedNeighbors = current.Neighbors().Where(n => !n.Links.Any());

                if (unvisitedNeighbors.Any())
                {
                    var neighbor = unvisitedNeighbors.Sample();
                    current.Link(neighbor);
                    current = neighbor;
                }
                else
                {
                    current = null;

                    foreach (var cell in grid.EachCell())
                    {
                        var visitedNeighbors = cell.Neighbors().Where(n => n.Links.Any());
                        if (!cell.Links.Any() && visitedNeighbors.Any())
                        {
                            current = cell;

                            var neighbor = visitedNeighbors.Sample();
                            current.Link(neighbor);

                            break;
                        }
                    }
                }
            }

            return grid;
        }
    }
}
