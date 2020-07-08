using Mazes;
using Mazes.Algorithms;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace SphereDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var grid = new SphereGrid(20);
            new RecursiveBacktracker().On(grid);

            var image = await grid.ToImageAsync();
            image.Save("sphere-map.png", ImageFormat.Png);
        }
    }
}
