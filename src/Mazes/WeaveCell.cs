namespace Mazes
{
    public abstract class WeaveCell : Cell
    {
        public WeaveCell(int row, int column) : base(row, column)
        {
        }
        public abstract bool HorizontalPassage { get; }

        public abstract bool VerticalPassage { get; }

    }
}
