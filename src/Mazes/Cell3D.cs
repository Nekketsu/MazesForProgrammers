using System.Collections.Generic;
using System.Linq;

namespace Mazes
{
    public class Cell3D : Cell
    {
        public int Level { get; }
        public Cell Up { get; set; }
        public Cell Down { get; set; }

        public Cell3D(int level, int row, int column) : base(row, column)
        {
            Level = level;
        }

        public override IEnumerable<Cell> Neighbors()
        {
            var neighboors = base.Neighbors()
                .Concat(new[] { Up, Down }.Where(c => c != null));

            return neighboors;
        }
    }
}
