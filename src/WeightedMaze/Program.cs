using Mazes;
using Mazes.Algorithms;
using Mazes.Extensions;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace WeightedMaze
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var grid = new WeightedGrid(10, 10);
            new RecursiveBacktracker().On(grid);

            grid.Braid(0.5f);

            var start = grid[0, 0];
            var finish = grid[grid.Rows - 1, grid.Columns - 1];

            grid.Distances = start.Distances().PathTo(finish);
            var image = await grid.ToImageAsync();
            image.Save("original.png", ImageFormat.Png);

            var lava = (WeightedCell)grid.Distances.Cells.Sample();
            lava.Weight = 50;

            grid.Distances = start.Distances().PathTo(finish);
            image = await grid.ToImageAsync();
            image.Save("rerouted.png", ImageFormat.Png);
        }
    }
}
