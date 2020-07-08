using System.Collections.Generic;
using System.Linq;

namespace Mazes
{
    public class PolarCell : Cell
    {
        public PolarCell Cw { get; set; }
        public PolarCell Ccw { get; set; }
        public PolarCell Inward { get; set; }
        public List<PolarCell> Outward { get; }


        public PolarCell(int row, int column) : base(row, column)
        {
            Outward = new List<PolarCell>();
        }

        public override IEnumerable<Cell> Neighbors()
        {
            var neighbors = new[] { Cw, Ccw, Inward }.Where(n => n != null).ToList();
            neighbors.AddRange(Outward);

            return neighbors;
        }
    }
}
