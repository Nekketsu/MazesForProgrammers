using Mazes;
using Mazes.Algorithms;
using System;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace SidewinderDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var grid = new Grid(4, 4);
            new Sidewinder().On(grid);

            Console.WriteLine(grid);

            var image = await grid.ToImageAsync();
            image.Save("maze.png", ImageFormat.Png);
        }
    }
}
