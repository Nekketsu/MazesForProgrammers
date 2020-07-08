using Mazes;
using Mazes.Algorithms;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace Coloring
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var grid = new ColoredGrid(25, 25);
            new BinaryTree().On(grid);

            var start = grid[grid.Rows / 2, grid.Columns / 2];

            grid.Distances = start.Distances();

            var image = await grid.ToImageAsync();
            image.Save("Maze.png", ImageFormat.Png);
        }
    }
}
