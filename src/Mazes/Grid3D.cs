using Mazes.Extensions;
using Mazes.Renderers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Mazes
{
    public class Grid3D : IGrid
    {
        Random random;

        public int Levels { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }

        public int Size => Levels * Rows * Columns;

        Cell3D[][][] grid;

        public Grid3D([DefaultValue(3)] int levels, [DefaultValue(3)] int rows, [DefaultValue(3)] int columns)
        {
            random = new Random();

            Levels = levels;
            Rows = rows;
            Columns = columns;

            PrepareGrid();
            ConfigureCells();
        }

        private Cell[][][] PrepareGrid()
        {
            grid = new Cell3D[Levels][][];

            for (var level = 0; level < Levels; level++)
            {
                grid[level] = new Cell3D[Rows][];

                for (var row = 0; row < Rows; row++)
                {
                    grid[level][row] = new Cell3D[Columns];
                    for (var column = 0; column < Columns; column++)
                    {
                        grid[level][row][column] = new Cell3D(level, row, column);
                    }
                }
            }

            return grid;
        }

        private void ConfigureCells()
        {
            foreach (Cell3D cell in EachCell())
            {
                cell.North = this[cell.Level, cell.Row - 1, cell.Column];
                cell.South = this[cell.Level, cell.Row + 1, cell.Column];
                cell.West = this[cell.Level, cell.Row, cell.Column - 1];
                cell.East = this[cell.Level, cell.Row, cell.Column + 1];
                cell.Down = this[cell.Level - 1, cell.Row, cell.Column];
                cell.Up = this[cell.Level + 1, cell.Row, cell.Column];
            }
        }

        public Cell3D this[int level, int row, int column]
        {
            get
            {
                if (level < 0 || level >= Levels ||
                    row < 0 || row >= Rows ||
                    column < 0 || column >= Columns)
                {
                    return null;
                }

                return grid[level][row][column];
            }
        }

        public virtual Cell RandomCell()
        {
            var level = random.Next(Levels);
            var row = random.Next(Rows);
            var column = random.Next(Columns);

            var randomCell = this[level, row, column];

            return randomCell;
        }


        public IEnumerable<Cell[][]> EachLevel()
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

        public virtual async Task<Image> ToImageAsync(int cellSize = 10, int inset = 0, int margin = -1)
        {
            var renderer = new ImageRenderer();

            await RenderAsync(renderer, cellSize, inset, margin);

            return renderer.Image;
        }

        public virtual async Task RenderAsync(IRenderer renderer, int cellSize = 10, int inset = 0)
        {
            await RenderAsync(renderer, cellSize, inset, -1);
        }

        public virtual async Task RenderAsync(IRenderer renderer, int cellSize = 10, int inset = 0, int margin = -1)
        {
            var paintSteps = new[]
            {
                PaintStep.Backgrounds,
                PaintStep.Walls
            };

            if (margin < 0)
            {
                margin = cellSize / 2;
            }

            var gridWidth = cellSize * Columns;
            var gridHeight = cellSize * Rows;

            var imgWidth = gridWidth * Levels + (Levels - 1) * margin;
            var imgHeight = gridHeight;

            var background = Color.White;
            var wall = Color.Black;
            var arrow = Color.Red;

            using (var graphics = await renderer.CreateAsync(imgWidth + 1, imgHeight + 1))
            {
                await graphics.ClearAsync(background);

                foreach (var paintStep in paintSteps)
                {
                    foreach (var cell in EachCell().Cast<Cell3D>())
                    {
                        var x = cell.Level * (gridWidth + margin) + cell.Column * cellSize;
                        var y = cell.Row * cellSize;

                        if (inset > 0)
                        {
                            await RenderWithInsetsAsync(wall, graphics, paintStep, cell, cellSize, x, y, inset);
                        }
                        else
                        {
                            await RenderWithoutInsetsAsync(wall, graphics, paintStep, cell, cellSize, x, y);
                        }

                        if (paintStep == PaintStep.Walls)
                        {
                            var midX = x + cellSize / 2;
                            var midY = y + cellSize / 2;

                            if (cell.Linked(cell.Down))
                            {
                                await graphics.DrawLineAsync(arrow, midX - 3, midY, midX - 1, midY + 2);
                                await graphics.DrawLineAsync(arrow, midX - 3, midY, midX - 1, midY - 2);
                            }

                            if (cell.Linked(cell.Up))
                            {
                                await graphics.DrawLineAsync(arrow, midX + 3, midY, midX + 1, midY + 2);
                                await graphics.DrawLineAsync(arrow, midX + 3, midY, midX + 1, midY - 2);
                            }
                        }
                    }
                }
            }
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

        private async Task RenderWithoutInsetsAsync(Color wall, IGraphics graphics, PaintStep paintStep, Cell cell, int cellSize, int x, int y)
        {
            var x1 = x;
            var y1 = y;
            var x2 = x1 + cellSize;
            var y2 = y1 + cellSize;

            if (paintStep == PaintStep.Walls)
            {
                if (cell.North == null)
                {
                    await graphics.DrawLineAsync(wall, x1, y1, x2, y1);
                }
                if (cell.West == null)
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

        public virtual Color BackgroundColorFor(Cell cell) => default;

        private (int x1, int x2, int x3, int x4, int y1, int y2, int y3, int y4) CellCoordinatesWithInset(int x, int y, int cellSize, int inset)
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

        public void Braid(float p = 1f)
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
    }
}
