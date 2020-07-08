using Mazes;
using Mazes.Algorithms;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace CylinderDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var grid = new CylinderGrid(7, 16);
            new RecursiveBacktracker().On(grid);

            var image = await grid.ToImageAsync();
            image.Save("cylinder.png", ImageFormat.Png);
        }
    }
}
