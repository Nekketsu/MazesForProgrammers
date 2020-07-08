using Mazes;
using Mazes.Algorithms;
using System;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace KruskalsWeaveDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var random = new Random();

            var grid = new PreconfiguredGrid(20, 20);
            var state = new Kruskals.State(grid);

            for (var i = 0; i < grid.Size; i++)
            {
                var row = random.Next(grid.Rows - 2);
                var column = random.Next(grid.Columns - 2);
                state.AddCrossing((OverCell)grid[row, column]);
            }

            new Kruskals().On(grid, state);

            var image = await grid.ToImageAsync(inset: 2);
            image.Save("kruskals.png", ImageFormat.Png);
        }
    }
}
