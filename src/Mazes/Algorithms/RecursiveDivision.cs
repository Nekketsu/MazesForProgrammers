using System;

namespace Mazes.Algorithms
{
    public class RecursiveDivision : IAlgorithm
    {
        Random random;
        Grid grid;

        public RecursiveDivision()
        {
            random = new Random();
        }

        public IGrid On(IGrid grid)
        {
            if (grid is Grid gridGrid)
            {
                return On(gridGrid);
            }

            return grid;
        }

        public Grid On(Grid grid)
        {
            this.grid = grid;

            foreach (var cell in grid.EachCell())
            {
                foreach (var neighbor in cell.Neighbors())
                {
                    cell.Link(neighbor, false);
                }
            }

            Divide(0, 0, grid.Rows, grid.Columns);

            return grid;
        }

        private void Divide(int row, int column, int height, int width)
        {
            if (ShouldCreateRoom(height, width))
            {
                return;
            }

            if (height > width)
            {
                DivideHorizontally(row, column, height, width);
            }
            else
            {
                DivideVertically(row, column, height, width);
            }
        }

        private void DivideHorizontally(int row, int column, int height, int width)
        {
            var divideSouthOf = random.Next(height - 1);
            var passageAt = random.Next(width);

            for (var x = 0; x < width; x++)
            {
                if (passageAt == x)
                {
                    continue;
                }

                var cell = grid[row + divideSouthOf, column + x];
                cell.Unlink(cell.South);
            }

            Divide(row, column, divideSouthOf + 1, width);
            Divide(row + divideSouthOf + 1, column, height - divideSouthOf - 1, width);
        }

        private void DivideVertically(int row, int column, int height, int width)
        {
            var divideEastOf = random.Next(width - 1);
            var passageAt = random.Next(height);

            for (var y = 0; y < height; y++)
            {
                if (passageAt == y)
                {
                    continue;
                }

                var cell = grid[row + y, column + divideEastOf];
                cell.Unlink(cell.East);
            }

            Divide(row, column, height, divideEastOf + 1);
            Divide(row, column + divideEastOf + 1, height, width - divideEastOf - 1);
        }

        public virtual bool ShouldCreateRoom(int height, int width)
        {
            return height <= 1 || width <= 1;
        }
    }
}
