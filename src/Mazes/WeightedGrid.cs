using System.ComponentModel;
using System.Drawing;

namespace Mazes
{
    public class WeightedGrid : Grid
    {
        Distances distances;
        int maximum;

        public WeightedGrid([DefaultValue(10)] int rows, [DefaultValue(10)] int columns) : base(rows, columns) { }

        public Distances Distances
        {
            get => distances;
            set
            {
                distances = value;
                (_, maximum) = distances.Max();
            }
        }

        protected override Cell[][] PrepareGrid()
        {
            grid = new WeightedCell[Rows][];

            for (var row = 0; row < Rows; row++)
            {
                grid[row] = new WeightedCell[Columns];
                for (var column = 0; column < Columns; column++)
                {
                    grid[row][column] = new WeightedCell(row, column);
                }
            }

            return grid;
        }

        public override Color BackgroundColorFor(Cell cell)
        {
            var weightedCell = (WeightedCell)cell;

            if (weightedCell.Weight > 1)
            {
                return Color.Red;
            }
            else if (distances != null)
            {
                var distance = distances[cell];
                if (distance != -1)
                {
                    var intensity = 64 + 191 * (maximum - distance) / maximum;
                    return Color.FromArgb(intensity, intensity, 0);
                }
            }

            return default;
        }
    }
}
