using Mazes;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace PolarGridTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var grid = new PolarGrid(8);

            var image = await grid.ToImageAsync();
            image.Save("Polar.png", ImageFormat.Png);
        }
    }
}
