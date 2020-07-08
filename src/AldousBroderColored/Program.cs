using Mazes;
using Mazes.Algorithms;
using System.Threading.Tasks;

namespace AldousBroderColored
{
    class Program
    {
        static async Task Main(string[] args)
        {
            for (var i = 0; i < 6; i++)
            {
                var grid = new ColoredGrid(20, 20);
                new AldousBroder().On(grid);

                var middle = grid[grid.Rows / 2, grid.Columns / 2];
                grid.Distances = middle.Distances();

                var image = await grid.ToImageAsync();
                image.Save($"AldousBroder_{i:D2}.png");
            }
        }
    }
}
