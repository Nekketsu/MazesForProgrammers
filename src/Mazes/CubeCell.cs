namespace Mazes
{
    public class CubeCell : Cell
    {
        public int Face { get; }

        public CubeCell(int face, int row, int column) : base(row, column)
        {
            Face = face;
        }
    }
}
