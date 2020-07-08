namespace Mazes
{
    public class HemisphereCell : PolarCell
    {
        public int Hemisphere { get; }

        public HemisphereCell(int hemisphere, int row, int column) : base(row, column)
        {
            Hemisphere = hemisphere;
        }
    }
}
