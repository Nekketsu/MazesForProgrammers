using System.Collections.Generic;
using System.Linq;

namespace Mazes
{
    public class SimpleOverCell : OverCell
    {
        public SimpleOverCell(int row, int column, WeaveGrid grid) : base(row, column, grid) { }

        public override IEnumerable<Cell> Neighbors()
        {
            var neighbors = new[] { North, South, East, West };

            return neighbors.Where(neighboor => neighboor != null);
        }
    }
}
