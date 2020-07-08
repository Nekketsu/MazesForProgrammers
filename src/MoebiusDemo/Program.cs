using Mazes;
using Mazes.Algorithms;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace MoebiusDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var grid = new MoebiusGrid(5, 50);
            new RecursiveBacktracker().On(grid);

            var image = await grid.ToImageAsync();
            image.Save("moebius.png", ImageFormat.Png);
        }
    }
}
