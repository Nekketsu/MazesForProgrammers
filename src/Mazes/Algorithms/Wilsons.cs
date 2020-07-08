using Mazes.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Mazes.Algorithms
{
    public class Wilsons : IAlgorithm
    {
        public IGrid On(IGrid grid)
        {
            var unvisited = grid.EachCell().ToList();

            var first = unvisited.Sample();
            unvisited.Remove(first);

            while (unvisited.Any())
            {
                var cell = unvisited.Sample();
                var path = new List<Cell> { cell };

                while (unvisited.Contains(cell))
                {
                    cell = cell.Neighbors().Sample();
                    var position = path.IndexOf(cell);

                    if (position != -1)
                    {
                        path = path.Take(position).ToList();
                    }
                    else
                    {
                        path.Add(cell);
                    }
                }

                for (var index = 0; index < path.Count - 1; index++)
                {
                    path[index].Link(path[index + 1]);
                    unvisited.Remove(path[index]);
                }
            }

            return grid;
        }
    }
}
