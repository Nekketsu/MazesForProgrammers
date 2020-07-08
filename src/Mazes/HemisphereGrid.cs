using System;
using System.ComponentModel;
using System.Linq;

namespace Mazes
{
    public class HemisphereGrid : PolarGrid
    {
        public int Id { get; }

        public HemisphereGrid([DefaultValue(0)] int id, [DefaultValue(10)] int rows)
        {
            Id = id;
            Rows = rows;
            Columns = 1;

            grid = PrepareGrid();
            ConfigureCells();
        }

        public int GetSize(int row)
        {
            return grid[row].Length;
        }

        protected override Cell[][] PrepareGrid()
        {
            var grid = new Cell[Rows][];

            var angularHeight = Math.PI / (2 * Rows);

            grid[0] = new[] { new HemisphereCell(Id, 0, 0) };

            for (var row = 1; row < Rows; row++)
            {
                var theta = (row + 1) * angularHeight;
                var radius = Math.Sin(theta);
                var circunference = 2 * Math.PI * radius;

                var previousCount = grid[row - 1].Length;
                var estimatedCellWidth = circunference / previousCount;
                var ratio = Math.Round(estimatedCellWidth / angularHeight);

                var cells = (int)(previousCount * ratio);
                grid[row] = Enumerable.Range(0, cells).Select(column => new HemisphereCell(Id, row, column)).ToArray();
            }

            return grid;
        }
    }
}
