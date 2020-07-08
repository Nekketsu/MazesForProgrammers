namespace Mazes.Services.Models.Parameters
{
    public class IntParameter : MazeParameter
    {
        public override object ObjectValue => Value;

        public string StringValue
        {
            get => Value.ToString();
            set => Value = int.Parse(value);
        }

        public int Value { get; set; }
        public int? Min { get; set; }

        public IntParameter(string name, int value) : base(typeof(int), name)
        {
            Value = value;
        }
    }
}
