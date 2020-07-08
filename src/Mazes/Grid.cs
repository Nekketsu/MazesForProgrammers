using Mazes.Extensions;
using Mazes.Renderers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mazes
{
    public partial class Grid : IGrid
    {
        public int Rows { get; protected set; }
        public int Columns { get; protected set; }
        public virtual int Size => Rows * Columns;

        protected Cell[][] grid;

        protected Random random;

        protected Grid()
        {
            random = new Random();
        }

        public Grid([DefaultValue(25)] int rows, [DefaultValue(25)] int columns) : this()
        {
            Rows = rows;
            Columns = columns;

            grid = PrepareGrid();
            ConfigureCells();
        }

        public virtual Cell this[int row, int column]
        {
            get
            {
                if (row < 0 || row >= Rows ||
                    column < 0 || column >= Columns)
                {
                    return null;
                }

                return grid[row][column];
            }
        }

        public virtual Cell RandomCell()
        {
            var row = random.Next(Rows);
            var column = random.Next(Columns);

            var randomCell = this[row, column];

            return randomCell;
        }

        public IEnumerable<Cell[]> EachRow()
        {
            return grid.AsEnumerable();
        }

        public virtual IEnumerable<Cell> EachCell()
        {
            return grid.SelectMany().Where(cell => cell != null);
        }

        public virtual string ContentsOf(Cell cell) => " ";

        public virtual Color BackgroundColorFor(Cell cell) => default;

        public override string ToString()
        {
            var output = new StringBuilder();
            output.AppendLine($"+{string.Concat(Enumerable.Repeat("---+", Columns))}");

            foreach (var row in EachRow())
            {
                var top = new StringBuilder("|");
                var bottom = new StringBuilder("+");

                foreach (var rowCell in row)
                {
                    var cell = rowCell ?? new Cell(-1, -1);
                    var body = $" {ContentsOf(cell)} ";
                    var eastBoundary = cell.Linked(cell.East) ? " " : "|";
                    top.Append(body + eastBoundary);

                    var southBoundary = cell.Linked(cell.South) ? "   " : "---";
                    var corner = "+";
                    bottom.Append(southBoundary + corner);
                }

                output.AppendLine(top.ToString());
                output.AppendLine(bottom.ToString());
            }

            return output.ToString();
        }

        public virtual async Task RenderAsync(IRenderer renderer, int cellSize = 10, int inset = 0)
        {
            var paintSteps = new[]
            {
                PaintStep.Backgrounds,
                PaintStep.Walls
            };

            var imgWidth = cellSize * Columns;
            var imgHeight = cellSize * Rows;

            var background = Color.White;
            var wall = Color.Black;

            using (var graphics = await renderer.CreateAsync(imgWidth + 1, imgHeight + 1))
            {
                await graphics.ClearAsync(background);

                foreach (var paintStep in paintSteps)
                {
                    foreach (var cell in EachCell())
                    {
                        var x = cell.Column * cellSize;
                        var y = cell.Row * cellSize;

                        if (inset > 0)
                        {
                            await RenderWithInsetsAsync(graphics, paintStep, cell, cellSize, wall, x, y, inset);
                        }
                        else
                        {
                            await RenderWithoutInsetsAsync(graphics, paintStep, cell, cellSize, wall, x, y);
                        }
                    }
                }
            }
        }

        protected virtual async Task RenderWithInsetsAsync(IGraphics graphics, PaintStep paintStep, Cell cell, int cellSize, Color wall, int x, int y, int inset)
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

        protected async Task RenderWithoutInsetsAsync(IGraphics graphics, PaintStep paintStep, Cell cell, int cellSize, Color wall, int x, int y)
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

        public virtual async Task<Image> ToImageAsync(int cellSize = 10, int inset = 0)
        {
            var renderer = new ImageRenderer();

            await RenderAsync(renderer, cellSize, inset);

            return renderer.Image;
        }

        protected virtual Cell[][] PrepareGrid()
        {
            grid = new Cell[Rows][];

            for (var row = 0; row < Rows; row++)
            {
                grid[row] = new Cell[Columns];
                for (var column = 0; column < Columns; column++)
                {
                    grid[row][column] = new Cell(row, column);
                }
            }

            return grid;
        }

        protected virtual void ConfigureCells()
        {
            foreach (var cell in EachCell())
            {
                var row = cell.Row;
                var column = cell.Column;

                cell.North = this[row - 1, column];
                cell.South = this[row + 1, column];
                cell.East = this[row, column + 1];
                cell.West = this[row, column - 1];
            }
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
