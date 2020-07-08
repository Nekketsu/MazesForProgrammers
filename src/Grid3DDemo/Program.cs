using Mazes;
using Mazes.Algorithms;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace Grid3DDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var grid = new Grid3D(3, 3, 3);
            new RecursiveBacktracker().On(grid);

            var image = await grid.ToImageAsync(30);
            image.Save("3d.png", ImageFormat.Png);
        }
    }
}
