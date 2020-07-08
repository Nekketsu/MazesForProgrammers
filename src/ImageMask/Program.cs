using Mazes;
using Mazes.Algorithms;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace ImageMask
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var file = args.Length >= 1 ? args[0] : "maze_text.png";

            var mask = Mask.FromImage(file);
            var grid = new MaskedGrid(mask);
            new RecursiveBacktracker().On(grid);

            var image = await grid.ToImageAsync(5);
            image.Save("masked.png", ImageFormat.Png);
        }
    }
}
