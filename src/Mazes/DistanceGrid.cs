using Mazes.Extensions;
using System.ComponentModel;

namespace Mazes
{
    public class DistanceGrid : Grid
    {
        public Distances Distances { get; set; }

        public DistanceGrid([DefaultValue(5)] int rows, [DefaultValue(5)] int columns) : base(rows, columns)
        {
        }

        public override string ContentsOf(Cell cell)
        {
            if ((Distances?[cell] ?? -1) >= 0)
            {
                return Distances[cell].ToBase36String();
            }
            else
            {
                return base.ContentsOf(cell);
            }
        }
    }
}
