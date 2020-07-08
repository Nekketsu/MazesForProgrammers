using System.Collections.Generic;
using System.Linq;

namespace Mazes
{
    public class TriangleCell : Cell
    {
        public TriangleCell(int row, int column) : base(row, column)
        {
        }

        public bool IsUpright()
        {
            return (Row + Column) % 2 == 0;
        }

        public override IEnumerable<Cell> Neighbors()
        {
            var neighbors = new[] { East, West }.AsEnumerable();

            neighbors = neighbors.Where(neighboor => neighboor != null);
            if (!IsUpright() && North != null)
            {
                neighbors = neighbors.Append(North);
            }
            if (IsUpright() && South != null)
            {
                neighbors = neighbors.Append(South);
            }

            return neighbors;
        }
    }
}
