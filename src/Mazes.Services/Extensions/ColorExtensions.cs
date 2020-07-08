using System.Drawing;
using System.Globalization;

namespace Mazes.Services.Extensions
{
    public static class ColorExtensions
    {
        public static string ToWebRgbaColor(this Color color)
        {
            return $"rgba({color.R}, {color.G}, {color.B}, {color.A})";
        }

        public static string ToWebRgbColor(this Color color)
        {
            return $"#{color.R:x2}{color.G:x2}{color.B:x2}";
        }

        public static Color ToColor(this string color)
        {
            if (color.StartsWith("#"))
            {
                var hexColor = color.Substring(1);
                var argb = int.Parse(hexColor, NumberStyles.HexNumber);

                return Color.FromArgb(argb);
            }

            return default;
        }
    }
}
