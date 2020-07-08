using Mazes;
using Mazes.Algorithms;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace AsciiMask
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var file = args.Length >= 1 ? args[0] : "mask.txt";

            var mask = Mask.FromTxt(file);
            if (mask == null)
            {
                return;
            }

            var grid = new MaskedGrid(mask);
            new RecursiveBacktracker().On(grid);

            var image = await grid.ToImageAsync();
            image.Save("masked.png", ImageFormat.Png);
        }
    }
}
