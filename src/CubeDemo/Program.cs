using Mazes;
using Mazes.Algorithms;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace CubeDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var grid = new CubeGrid(10);
            new RecursiveBacktracker().On(grid);

            var image = await grid.ToImageAsync();
            image.Save("cube.png", ImageFormat.Png);
        }
    }
}
