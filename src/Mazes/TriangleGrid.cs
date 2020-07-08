using Mazes.Renderers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Mazes
{
    public class TriangleGrid : Grid
    {
        public TriangleGrid([DefaultValue(10)] int rows, [DefaultValue(17)] int columns) : base(rows, columns)
        {
        }

        protected override Cell[][] PrepareGrid()
        {
            grid = new Cell[Rows][];

            for (var row = 0; row < Rows; row++)
            {
                grid[row] = new TriangleCell[Columns];
                for (var column = 0; column < Columns; column++)
                {
                    grid[row][column] = new TriangleCell(row, column);
                }
            }

            return grid;
        }

        protected override void ConfigureCells()
        {
            foreach (var cell in EachCell().Cast<TriangleCell>())
            {
                var row = cell.Row;
                var column = cell.Column;

                cell.West = this[row, column - 1];
                cell.East = this[row, column + 1];

                if (cell.IsUpright())
                {
                    cell.South = this[row + 1, column];
                }
                else
                {
                    cell.North = this[row - 1, column];
                }
            }
        }

        public override async Task RenderAsync(IRenderer renderer, int size = 16, int inset = 0)
        {
            var paintSteps = new List<PaintStep>
            {
                PaintStep.Backgrounds,
                PaintStep.Walls
            };

            var halfWidth = size / 2.0;
            var height = size * Math.Sqrt(3) / 2.0;
            var halfHeight = height / 2.0;

            var imgWidth = (int)(size * (Columns + 1) / 2.0);
            var imgHeight = (int)(height * Rows);

            var background = Color.White;
            var wall = Color.Black;

            using (var graphics = await renderer.CreateAsync(imgWidth + 1, imgHeight + 1))
            {
                await graphics.ClearAsync(background);

                foreach (var paintStep in paintSteps)
                {
                    foreach (var cell in EachCell().Cast<TriangleCell>())
                    {
                        var cx = halfWidth + cell.Column * halfWidth;
                        var cy = halfHeight + cell.Row * height;

                        var westX = (int)(cx - halfWidth);
                        var midX = (int)cx;
                        var eastX = (int)(cx + halfWidth);

                        int apexY;
                        int baseY;
                        if (cell.IsUpright())
                        {
                            apexY = (int)(cy - halfHeight);
                            baseY = (int)(cy + halfHeight);
                        }
                        else
                        {
                            apexY = (int)(cy + halfHeight);
                            baseY = (int)(cy - halfHeight);
                        }

                        if (paintStep == PaintStep.Walls)
                        {
                            if (cell.West == null)
                            {
                                await graphics.DrawLineAsync(wall, westX, baseY, midX, apexY);
                            }
                            if (!cell.Linked(cell.East))
                            {
                                await graphics.DrawLineAsync(wall, eastX, baseY, midX, apexY);
                            }

                            var noSouth = cell.IsUpright() && cell.South == null;
                            var notLinked = !cell.IsUpright() && !cell.Linked(cell.North);

                            if (noSouth || notLinked)
                            {
                                await graphics.DrawLineAsync(wall, eastX, baseY, westX, baseY);
                            }
                        }
                        else if (paintStep == PaintStep.Backgrounds)
                        {
                            var color = BackgroundColorFor(cell);
                            await graphics.FillPolygonAsync(color, new[]
                            {
                                new Point(westX, baseY),
                                new Point(midX, apexY),
                                new Point(eastX, baseY)
                            });
                        }
                    }
                }
            }
        }
    }
}
