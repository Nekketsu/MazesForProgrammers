using Mazes;
using Mazes.Algorithms;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace HexMaze
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var grid = new HexGrid(10, 10);
            new RecursiveBacktracker().On(grid);

            var image = await grid.ToImageAsync();
            image.Save("hex.png", ImageFormat.Png);
        }
    }
}
