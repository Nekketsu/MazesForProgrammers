using System.ComponentModel;
using System.Drawing;

namespace Mazes
{
    public class InterpolatedColoredGrid : Grid
    {
        public Color CloseColor { get; set; }
        public Color FarColor { get; set; }

        private Distances distances;
        private int maximum;

        public InterpolatedColoredGrid([DefaultValue(25)] int rows, [DefaultValue(25)] int columns, [DefaultValue("Red")] Color closeColor = default, [DefaultValue("Black")] Color farColor = default) : base(rows, columns)
        {
            CloseColor = closeColor == default ? Color.Green : closeColor;
            FarColor = farColor == default ? Color.Green : farColor;
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
            var intensity = ((float)distance) / maximum;
            var color = InterpolateColor(CloseColor, FarColor, intensity);

            return color;
        }

        private Color InterpolateColor(Color closeColor, Color farColor, float intensity)
        {
            var r = farColor.R - closeColor.R;
            var g = farColor.G - closeColor.G;
            var b = farColor.B - closeColor.B;

            var color = Color.FromArgb((byte)(closeColor.R + intensity * r), (byte)(closeColor.G + intensity * g), (byte)(closeColor.B + intensity * b));

            return color;
        }
    }
}
