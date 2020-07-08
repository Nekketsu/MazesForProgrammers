using Mazes.Extensions;
using Mazes.Renderers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Mazes
{
    public class CubeGrid : IGrid
    {
        Random random;

        const int Faces = 6;
        public int Dim { get; set; }

        public int Size => Faces * Dim * Dim;

        CubeCell[][][] grid;

        public CubeGrid([DefaultValue(10)] int dim)
        {
            random = new Random();

            Dim = dim;

            PrepareGrid();
            ConfigureCells();
        }

        private Cell[][][] PrepareGrid()
        {
            grid = new CubeCell[Faces][][];

            for (var face = 0; face < Faces; face++)
            {
                grid[face] = new CubeCell[Dim][];

                for (var row = 0; row < Dim; row++)
                {
                    grid[face][row] = new CubeCell[Dim];
                    for (var column = 0; column < Dim; column++)
                    {
                        grid[face][row][column] = new CubeCell(face, row, column);
                    }
                }
            }

            return grid;
        }

        public IEnumerable<Cell[][]> EachFace()
        {
            return grid.AsEnumerable();
        }

        public IEnumerable<Cell[]> EachRow()
        {
            return grid.SelectMany();
        }

        public IEnumerable<Cell> EachCell()
        {
            return grid.SelectMany(r => r.SelectMany().Where(c => c != null));
        }

        public virtual Cell RandomCell()
        {
            var level = random.Next(Faces);
            var row = random.Next(Dim);
            var column = random.Next(Dim);

            var randomCell = this[level, row, column];

            return randomCell;
        }

        private void ConfigureCells()
        {
            foreach (var cell in EachCell().Cast<CubeCell>())
            {
                cell.West = this[cell.Face, cell.Row, cell.Column - 1];
                cell.East = this[cell.Face, cell.Row, cell.Column + 1];
                cell.North = this[cell.Face, cell.Row - 1, cell.Column];
                cell.South = this[cell.Face, cell.Row + 1, cell.Column];
            }
        }

        public CubeCell this[int face, int row, int column]
        {
            get
            {
                if (face < 0 || face >= Faces)
                {
                    return null;
                }

                (face, row, column) = Wrap(face, row, column);

                return grid[face][row][column];
            }
        }

        public void Braid(float p = 1)
        {
            foreach (var cell in DeadEnds().Shuffle())
            {
                if (cell.Links.Count != 1 || random.NextDouble() > p)
                {
                    continue;
                }

                var neighbors = cell.Neighbors().Where(n => !cell.Linked(n));
                var best = neighbors.Where(n => n.Links.Count == 1);
                if (!best.Any())
                {
                    best = neighbors;
                }

                var neighbor = best.Sample();
                cell.Link(neighbor);
            }
        }

        public (int face, int row, int column) Wrap(int face, int row, int column)
        {
            var n = Dim - 1;

            if (row < 0)
            {
                return face switch
                {
                    0 => (4, column, 0),
                    1 => (4, n, column),
                    2 => (4, n - column, n),
                    3 => (4, 0, n - column),
                    4 => (3, 0, n - column),
                    5 => (1, n, column),
                    _ => (face, row, column)
                };
            }
            else if (row >= Dim)
            {
                return face switch
                {
                    0 => (5, n - column, 0),
                    1 => (5, 0, column),
                    2 => (5, column, n),
                    3 => (5, n, n - column),
                    4 => (1, 0, column),
                    5 => (3, n, n - column),
                    _ => (face, row, column)
                };
            }
            else if (column < 0)
            {
                return face switch
                {
                    0 => (3, row, n),
                    1 => (0, row, n),
                    2 => (1, row, n),
                    3 => (2, row, n),
                    4 => (0, 0, row),
                    5 => (0, n, n - row),
                    _ => (face, row, column)
                };
            }
            else if (column >= Dim)
            {
                return face switch
                {
                    0 => (1, row, 0),
                    1 => (2, row, 0),
                    2 => (3, row, 0),
                    3 => (0, row, 0),
                    4 => (2, 0, n - row),
                    5 => (2, n, row),
                    _ => (face, row, column)
                };
            }

            return (face, row, column);
        }

        public IEnumerable<Cell> DeadEnds()
        {
            var list = new List<Cell>();

            foreach (var cell in EachCell())
            {
                if (cell.Links.Count == 1)
                {
                    list.Add(cell);
                }
            }

            return list;
        }

        public virtual async Task<Image> ToImageAsync(int cellSize = 10, int inset = 0)
        {
            var renderer = new ImageRenderer();

            await RenderAsync(renderer, cellSize, inset);

            return renderer.Image;
        }

        public async Task RenderAsync(IRenderer renderer, int cellSize = 10, int inset = 0)
        {
            var paintSteps = new[]
               {
                PaintStep.Backgrounds,
                PaintStep.Walls
            };

            var faceWidth = cellSize * Dim;
            var faceHeight = cellSize * Dim;

            var imgWidth = 4 * faceWidth;
            var imgHeight = 3 * faceHeight;

            var offsets = new[]
            {
                new { X = 0, Y = 1 },
                new { X = 1, Y = 1 },
                new { X = 2, Y = 1 },
                new { X = 3, Y = 1 },
                new { X = 1, Y = 0 },
                new { X = 1, Y = 2 },
            };

            var background = Color.White;
            var wall = Color.Black;
            var outline = Color.FromArgb(0xd0, 0xd0, 0xd0);

            using (var graphics = await renderer.CreateAsync(imgWidth + 1, imgHeight + 1))
            {
                await graphics.ClearAsync(background);

                await DrawOutlines(graphics, faceWidth, faceHeight, outline);

                foreach (var paintStep in paintSteps)
                {
                    foreach (var cell in EachCell().Cast<CubeCell>())
                    {
                        var x = offsets[cell.Face].X * faceWidth + cell.Column * cellSize;
                        var y = offsets[cell.Face].Y * faceHeight + cell.Row * cellSize;

                        if (inset > 0)
                        {
                            await RenderWithInsetsAsync(wall, graphics, paintStep, cell, cellSize, x, y, inset);
                        }
                        else
                        {
                            await RenderWithoutInsetsAsync(wall, graphics, paintStep, cell, cellSize, x, y);
                        }
                    }
                }
            }
        }

        private async Task DrawOutlines(IGraphics graphics, int width, int height, Color outline)
        {
            // Horizontal faces #0, #1, #2, #3
            await graphics.DrawRectangleAsync(outline, 0, height, width * 4, height);

            // Vertical faces #1, #4, #5
            await graphics.DrawRectangleAsync(outline, width, 0, width, height * 3);

            // Line between faces #2 & #3
            await graphics.DrawLineAsync(outline, width * 3, height, width * 3, height * 2);
        }

        private async Task RenderWithInsetsAsync(Color wall, IGraphics graphics, PaintStep paintStep, Cell cell, int cellSize, int x, int y, int inset)
        {
            var (x1, x2, x3, x4, y1, y2, y3, y4) = CellCoordinatesWithInset(x, y, cellSize, inset);

            if (paintStep == PaintStep.Walls)
            {
                if (cell.Linked(cell.North))
                {
                    await graphics.DrawLineAsync(wall, x2, y1, x2, y2);
                    await graphics.DrawLineAsync(wall, x3, y1, x3, y2);
                }
                else
                {
                    await graphics.DrawLineAsync(wall, x2, y2, x3, y2);
                }

                if (cell.Linked(cell.South))
                {
                    await graphics.DrawLineAsync(wall, x2, y3, x2, y4);
                    await graphics.DrawLineAsync(wall, x3, y3, x3, y4);
                }
                else
                {
                    await graphics.DrawLineAsync(wall, x2, y3, x3, y3);
                }

                if (cell.Linked(cell.West))
                {
                    await graphics.DrawLineAsync(wall, x1, y2, x2, y2);
                    await graphics.DrawLineAsync(wall, x1, y3, x2, y3);
                }
                else
                {
                    await graphics.DrawLineAsync(wall, x2, y2, x2, y3);
                }

                if (cell.Linked(cell.East))
                {
                    await graphics.DrawLineAsync(wall, x3, y2, x4, y2);
                    await graphics.DrawLineAsync(wall, x3, y3, x4, y3);
                }
                else
                {
                    await graphics.DrawLineAsync(wall, x3, y2, x3, y3);
                }
            }
            else if (paintStep == PaintStep.Backgrounds)
            {
                var color = BackgroundColorFor(cell);
                await graphics.FillRectangleAsync(color, x2, y2, x3, y3);

                if (cell.Linked(cell.North))
                {
                    await graphics.FillRectangleAsync(color, x2, y1, x3, y2);
                }
                if (cell.Linked(cell.South))
                {
                    await graphics.FillRectangleAsync(color, x2, y3, x3, y4);
                }
                if (cell.Linked(cell.West))
                {
                    await graphics.FillRectangleAsync(color, x1, y2, x2, y3);
                }
                if (cell.Linked(cell.East))
                {
                    await graphics.FillRectangleAsync(color, x3, y2, x4, y3);
                }
            }
        }

        private async Task RenderWithoutInsetsAsync(Color wall, IGraphics graphics, PaintStep paintStep, CubeCell cell, int cellSize, int x, int y)
        {
            var x1 = x;
            var y1 = y;
            var x2 = x1 + cellSize;
            var y2 = y1 + cellSize;

            if (paintStep == PaintStep.Walls)
            {
                if (((CubeCell)cell.North).Face != cell.Face && !cell.Linked(cell.North))
                {
                    await graphics.DrawLineAsync(wall, x1, y1, x2, y1);
                }

                if (((CubeCell)cell.West).Face != cell.Face && !cell.Linked(cell.West))
                {
                    await graphics.DrawLineAsync(wall, x1, y1, x1, y2);
                }

                if (!cell.Linked(cell.East))
                {
                    await graphics.DrawLineAsync(wall, x2, y1, x2, y2);
                }
                if (!cell.Linked(cell.South))
                {
                    await graphics.DrawLineAsync(wall, x1, y2, x2, y2);
                }
            }
            else if (paintStep == PaintStep.Backgrounds)
            {
                var color = BackgroundColorFor(cell);
                await graphics.FillRectangleAsync(color, x1, y1, x2, y2);
            }
        }

        protected (int x1, int x2, int x3, int x4, int y1, int y2, int y3, int y4) CellCoordinatesWithInset(int x, int y, int cellSize, int inset)
        {
            int x1 = x;
            int x4 = x + cellSize;
            int x2 = x1 + inset;
            int x3 = x4 - inset;

            int y1 = y;
            int y4 = y + cellSize;
            int y2 = y1 + inset;
            int y3 = y4 - inset;

            return (x1, x2, x3, x4, y1, y2, y3, y4);
        }

        private Color BackgroundColorFor(Cell cell) => default;
    }
}
