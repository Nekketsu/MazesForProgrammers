using Mazes.Renderers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Mazes
{
    public class WeaveGrid : Grid
    {
        List<UnderCell> underCells;

        public WeaveGrid([DefaultValue(20)] int rows, [DefaultValue(20)] int columns) : base()
        {
            underCells = new List<UnderCell>();

            Rows = rows;
            Columns = columns;

            grid = PrepareGrid();
            ConfigureCells();
        }

        protected override Cell[][] PrepareGrid()
        {
            grid = new OverCell[Rows][];

            for (var row = 0; row < Rows; row++)
            {
                grid[row] = new OverCell[Columns];
                for (var column = 0; column < Columns; column++)
                {
                    grid[row][column] = new OverCell(row, column, this);
                }
            }

            return grid;
        }

        public UnderCell TunnelUnder(OverCell overCell)
        {
            var underCell = new UnderCell(overCell);
            underCells.Add(underCell);

            return underCell;
        }

        public override IEnumerable<Cell> EachCell()
        {
            return base.EachCell().Concat(underCells);
        }

        public override Task RenderAsync(IRenderer renderer, int cellSize = 10, int inset = 1)
        {
            return base.RenderAsync(renderer, cellSize, inset);
        }

        protected override async Task RenderWithInsetsAsync(IGraphics graphics, PaintStep paintStep, Cell cell, int cellSize, Color wall, int x, int y, int inset)
        {
            if (cell is UnderCell underCell)
            {
                var (x1, x2, x3, x4, y1, y2, y3, y4) = CellCoordinatesWithInset(x, y, cellSize, inset);

                if (underCell.VerticalPassage)
                {
                    await graphics.DrawLineAsync(wall, x2, y1, x2, y2);
                    await graphics.DrawLineAsync(wall, x3, y1, x3, y2);
                    await graphics.DrawLineAsync(wall, x2, y3, x2, y4);
                    await graphics.DrawLineAsync(wall, x3, y3, x3, y4);
                }
                else
                {
                    await graphics.DrawLineAsync(wall, x1, y2, x2, y2);
                    await graphics.DrawLineAsync(wall, x1, y3, x2, y3);
                    await graphics.DrawLineAsync(wall, x3, y2, x4, y2);
                    await graphics.DrawLineAsync(wall, x3, y3, x4, y3);
                }
            }
            else
            {
                await base.RenderWithInsetsAsync(graphics, paintStep, cell, cellSize, wall, x, y, inset);
            }
        }

        public override Task<Image> ToImageAsync(int cellSize = 10, int inset = 1)
        {
            return base.ToImageAsync(cellSize, inset);
        }
    }
}
