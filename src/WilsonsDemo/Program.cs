using Mazes;
using Mazes.Algorithms;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace WilsonsDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var grid = new Grid(20, 20);
            new Wilsons().On(grid);

            var image = await grid.ToImageAsync();
            image.Save("Wilsons.png", ImageFormat.Png);
        }
    }
}
