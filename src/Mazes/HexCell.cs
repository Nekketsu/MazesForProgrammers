using System.Collections.Generic;
using System.Linq;

namespace Mazes
{
    public class HexCell : Cell
    {
        public HexCell(int row, int column) : base(row, column)
        {
        }

        public Cell Northeast { get; set; }
        public Cell Northwest { get; set; }
        public Cell Southeast { get; set; }
        public Cell Southwest { get; set; }

        public override IEnumerable<Cell> Neighbors()
        {
            var neighbors = new[] { Northwest, North, Northeast, Southwest, South, Southeast };

            return neighbors.Where(neighboor => neighboor != null);
        }
    }
}
