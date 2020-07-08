using Mazes.Extensions;
using System.Linq;

namespace Mazes.Algorithms
{
    public class BinaryTree : IAlgorithm
    {
        public IGrid On(IGrid grid)
        {
            foreach (var cell in grid.EachCell())
            {
                var neighbors = new[] { cell.North, cell.East }.Where(c => c != null).ToArray();

                if (neighbors.Any())
                {
                    var neighbor = neighbors.Sample();

                    cell.Link(neighbor);
                }
            }

            return grid;
        }
    }
}
