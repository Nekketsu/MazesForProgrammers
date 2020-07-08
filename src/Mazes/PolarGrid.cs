using Mazes.Renderers;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Mazes
{
    public class PolarGrid : Grid
    {
        public PolarGrid() { }

        public PolarGrid([DefaultValue(8)] int rows)
        {
            Rows = rows;
            Columns = 1;

            grid = PrepareGrid();
            ConfigureCells();
        }

        protected override Cell[][] PrepareGrid()
        {
            var rows = new PolarCell[Rows][];

            var rowHeight = 1.0 / Rows;
            rows[0] = new[] { new PolarCell(0, 0) };

            for (var row = 1; row < Rows; row++)
            {
                var radius = (double)row / Rows;
                var circumference = 2 * Math.PI * radius;

                var previousCount = rows[row - 1].Length;
                var estimatedCellWidth = circumference / previousCount;
                var ratio = (int)Math.Round(estimatedCellWidth / rowHeight);

                var cells = previousCount * ratio;
                rows[row] = Enumerable.Range(0, cells).Select(column => new PolarCell(row, column)).ToArray();
            }

            return rows;
        }

        protected override void ConfigureCells()
        {
            foreach (var cell in EachCell().Cast<PolarCell>())
            {
                var row = cell.Row;
                var column = cell.Column;

                if (row > 0)
                {
                    cell.Cw = (PolarCell)this[row, column + 1];
                    cell.Ccw = (PolarCell)this[row, column - 1];

                    var ratio = grid[row].Length / grid[row - 1].Length;
                    var parent = (PolarCell)grid[row - 1][column / ratio];
                    parent.Outward.Add(cell);
                    cell.Inward = parent;
                }
            }
        }

        public override Cell this[int row, int column]
        {
            get
            {
                if (row >= 0 && row <= Rows - 1)
                {
                    return grid[row][(column + grid[row].Length) % grid[row].Length];
                }
                else
                {
                    return null;
                }
            }
        }

        public override Cell RandomCell()
        {
            var row = random.Next(Rows);
            var column = random.Next(grid[row].Length);

            return grid[row][column];
        }

        public override async Task RenderAsync(IRenderer renderer, int cellSize = 10, int inset = 0)
        {
            var radius = Rows * cellSize;
            var imgSize = 2 * radius;

            var background = Color.White;
            var wall = Color.Black;

            using (var graphics = await renderer.CreateAsync(imgSize + 1, imgSize + 1))
            {
                await graphics.ClearAsync(background);
                var center = radius;

                foreach (var cell in EachCell().Cast<PolarCell>())
                {
                    if (cell.Row == 0) { continue; }

                    var thetaDegrees = 360.0f / grid[cell.Row].Length;
                    var theta = 2 * MathF.PI / grid[cell.Row].Length;
                    var innerRadius = cell.Row * cellSize;
                    var outerRadius = (cell.Row + 1) * cellSize;
                    //var thetaCcw = cell.Column * theta;
                    var thetaCw = (cell.Column + 1) * theta;
                    var thetaCcwDegrees = cell.Column * thetaDegrees;

                    //var ax = center + (int)(innerRadius * Math.Cos(thetaCcw));
                    //var ay = center + (int)(innerRadius * Math.Sin(thetaCcw));
                    //var bx = center + (int)(outerRadius * Math.Cos(thetaCcw));
                    //var by = center + (int)(outerRadius * Math.Sin(thetaCcw));
                    var cx = center + (int)(innerRadius * Math.Cos(thetaCw));
                    var cy = center + (int)(innerRadius * Math.Sin(thetaCw));
                    var dx = center + (int)(outerRadius * Math.Cos(thetaCw));
                    var dy = center + (int)(outerRadius * Math.Sin(thetaCw));

                    if (!cell.Linked(cell.Inward))
                    {
                        //graphics.DrawLine(wall, ax, ay, cx, cy);
                        var innerDiameter = 2 * innerRadius;
                        await graphics.DrawArcAsync(wall, center - innerRadius, center - innerRadius, innerDiameter, innerDiameter, thetaCcwDegrees, thetaDegrees);
                    }
                    if (!cell.Linked(cell.Cw))
                    {
                        await graphics.DrawLineAsync(wall, cx, cy, dx, dy);
                    }
                }

                await graphics.DrawEllipseAsync(wall, 0, 0, imgSize, imgSize);
            }
        }
    }
}
