using Mazes;
using Mazes.Algorithms;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace PrimsDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var grid = new Grid(20, 20);
            new SimplifiedPrims().On(grid);

            var image = await grid.ToImageAsync();
            image.Save("prims-simple.png", ImageFormat.Png);
        }
    }
}
