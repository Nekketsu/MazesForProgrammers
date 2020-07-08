using System;
using System.ComponentModel;
using System.Drawing;

namespace Mazes
{
    public class ColoredGrid : Grid
    {
        private Distances distances;
        private int maximum;

        public ColoredGrid([DefaultValue(25)] int rows, [DefaultValue(25)] int columns) : base(rows, columns)
        {
        }

        public Distances Distances
        {
            get => distances;
            set
            {
                distances = value;
                (_, maximum) = distances.Max();
            }
        }

        public override Color BackgroundColorFor(Cell cell)
        {
            if (Distances == null)
            {
                return default;
            }

            var distance = Distances[cell];
            var intensity = ((float)(maximum - distance)) / maximum;
            var dark = (int)Math.Round(255 * intensity);
            var bright = 128 + (int)Math.Round(127 * intensity);

            return Color.FromArgb(dark, bright, dark);
        }
    }
}
