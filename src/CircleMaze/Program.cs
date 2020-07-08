using Mazes;
using Mazes.Algorithms;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace CircleMaze
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var grid = new PolarGrid(8);
            new HuntAndKill().On(grid);

            var image = await grid.ToImageAsync();
            image.Save("CircleMaze.png", ImageFormat.Png);
        }
    }
}
