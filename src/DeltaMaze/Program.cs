using Mazes;
using Mazes.Algorithms;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace DeltaMaze
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var grid = new TriangleGrid(10, 17);
            new RecursiveBacktracker().On(grid);

            var image = await grid.ToImageAsync();
            image.Save("delta.png", ImageFormat.Png);
        }
    }
}
