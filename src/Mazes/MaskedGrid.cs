namespace Mazes
{
    public class MaskedGrid : Grid
    {
        public Mask Mask { get; }

        public MaskedGrid(Mask mask) : base()
        {
            Mask = mask;

            Rows = mask?.Rows ?? 0;
            Columns = mask?.Columns ?? 0;

            grid = PrepareGrid();
            ConfigureCells();
        }

        protected override Cell[][] PrepareGrid()
        {
            grid = new Cell[Rows][];

            for (var row = 0; row < Rows; row++)
            {
                grid[row] = new Cell[Columns];
                for (var column = 0; column < Columns; column++)
                {
                    if (Mask?[row, column] ?? true)
                    {
                        grid[row][column] = new Cell(row, column);
                    }
                }
            }

            return grid;
        }

        public override Cell RandomCell()
        {
            if (Mask != null)
            {
                var (row, column) = Mask.RandomLocation();

                return this[row, column];
            }
            else
            {
                return base.RandomCell();
            }
        }

        public override int Size => Mask?.Count() ?? base.Size;
    }
}
