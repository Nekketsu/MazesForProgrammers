using Mazes;
using Mazes.Algorithms;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace RecursiveBackgrackerDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var grid = new Grid(20, 20);
            new RecursiveBacktracker().On(grid);

            var image = await grid.ToImageAsync();
            image.Save("RecursiveBacktracker.png", ImageFormat.Png);
        }
    }
}
