using System.ComponentModel;

namespace Mazes
{
    public class CylinderGrid : Grid
    {
        public CylinderGrid([DefaultValue(7)] int rows, [DefaultValue(16)] int columns) : base(rows, columns) { }

        public override Cell this[int row, int column]
        {
            get
            {
                if (row < 0 || row >= Rows)
                {
                    return null;
                }
                column = (column + grid[row].Length) % grid[row].Length;

                return base[row, column];
            }
        }
    }
}
