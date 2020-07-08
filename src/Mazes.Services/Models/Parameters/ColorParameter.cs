using Mazes.Services.Extensions;
using System.Drawing;

namespace Mazes.Services.Models.Parameters
{
    public class ColorParameter : MazeParameter
    {
        public override object ObjectValue => Value;
        Color Value { get; set; }

        public string StringValue
        {
            get => Value.ToWebRgbColor();
            set => Value = value.ToColor();
        }

        public ColorParameter(string name, Color value) : base(typeof(Color), name)
        {
            Value = value;
        }
    }
}
