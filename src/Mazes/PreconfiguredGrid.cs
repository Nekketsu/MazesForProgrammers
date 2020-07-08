using System.ComponentModel;

namespace Mazes
{
    public class PreconfiguredGrid : WeaveGrid
    {
        public PreconfiguredGrid([DefaultValue(20)] int rows, [DefaultValue(20)] int columns) : base(rows, columns)
        {
        }

        protected override Cell[][] PrepareGrid()
        {
            grid = new SimpleOverCell[Rows][];

            for (var row = 0; row < Rows; row++)
            {
                grid[row] = new SimpleOverCell[Columns];
                for (var column = 0; column < Columns; column++)
                {
                    grid[row][column] = new SimpleOverCell(row, column, this);
                }
            }

            return grid;
        }
    }
}
