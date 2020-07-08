using Mazes.Renderers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Mazes
{
    public class HexGrid : Grid
    {
        public HexGrid([DefaultValue(10)] int rows, [DefaultValue(10)] int columns) : base(rows, columns)
        {
        }

        protected override Cell[][] PrepareGrid()
        {
            grid = new HexCell[Rows][];

            for (var row = 0; row < Rows; row++)
            {
                grid[row] = new HexCell[Columns];
                for (var column = 0; column < Columns; column++)
                {
                    grid[row][column] = new HexCell(row, column);
                }
            }

            return grid;
        }

        protected override void ConfigureCells()
        {
            foreach (var cell in EachCell().Cast<HexCell>())
            {
                var row = cell.Row;
                var column = cell.Column;

                int northDiagonal;
                int southDiagonal;
                if (column % 2 == 0)
                {
                    northDiagonal = row - 1;
                    southDiagonal = row;
                }
                else
                {
                    northDiagonal = row;
                    southDiagonal = row + 1;
                }

                cell.Northwest = this[northDiagonal, column - 1];
                cell.North = this[row - 1, column];
                cell.Northeast = this[northDiagonal, column + 1];
                cell.Southwest = this[southDiagonal, column - 1];
                cell.South = this[row + 1, column];
                cell.Southeast = this[southDiagonal, column + 1];
            }
        }

        public override async Task RenderAsync(IRenderer renderer, int size = 10, int inset = 0)
        {
            var paintSteps = new List<PaintStep>
            {
                PaintStep.Backgrounds,
                PaintStep.Walls
            };

            var aSize = size / 2.0;
            var bSize = size * Math.Sqrt(3) / 2.0;
            var width = size * 2;
            var height = bSize * 2;

            var imgWidth = (int)(3 * aSize * Columns + aSize + 0.5);
            var imgHeight = (int)(height * Rows + bSize + 0.5);

            var background = Color.White;
            var wall = Color.Black;

            using (var graphics = await renderer.CreateAsync(imgWidth + 1, imgHeight + 1))
            {
                await graphics.ClearAsync(background);

                foreach (var paintStep in paintSteps)
                {
                    foreach (var cell in EachCell().Cast<HexCell>())
                    {
                        var cx = size + 3 * cell.Column * aSize;
                        var cy = bSize + cell.Row * height;
                        if (cell.Column % 2 == 1)
                        {
                            cy += bSize;
                        }

                        // f/n = far/near
                        // n/s/e/w = north/south/east/west
                        var xFw = (int)(cx - size);
                        var xNw = (int)(cx - aSize);
                        var xNe = (int)(cx + aSize);
                        var xFe = (int)(cx + size);

                        // m = middle
                        var yN = (int)(cy - bSize);
                        var yM = (int)cy;
                        var yS = (int)(cy + bSize);

                        if (paintStep == PaintStep.Walls)
                        {
                            if (cell.Southwest == null)
                            {
                                await graphics.DrawLineAsync(wall, xFw, yM, xNw, yS);
                            }
                            if (cell.Northwest == null)
                            {
                                await graphics.DrawLineAsync(wall, xFw, yM, xNw, yN);
                            }
                            if (cell.North == null)
                            {
                                await graphics.DrawLineAsync(wall, xNw, yN, xNe, yN);
                            }
                            if (!cell.Linked(cell.Northeast))
                            {
                                await graphics.DrawLineAsync(wall, xNe, yN, xFe, yM);
                            }
                            if (!cell.Linked(cell.Southeast))
                            {
                                await graphics.DrawLineAsync(wall, xFe, yM, xNe, yS);
                            }
                            if (!cell.Linked(cell.South))
                            {
                                await graphics.DrawLineAsync(wall, xNe, yS, xNw, yS);
                            }
                        }
                        else if (paintStep == PaintStep.Backgrounds)
                        {
                            var color = BackgroundColorFor(cell);
                            await graphics.FillPolygonAsync(color, new[]
                            {
                                new Point(xFw, yM),
                                new Point(xNw, yN),
                                new Point(xNe, yN),
                                new Point(xFe, yM),
                                new Point(xNe, yS),
                                new Point(xNw, yS)
                            });
                        }
                    }
                }
            }
        }
    }
}
