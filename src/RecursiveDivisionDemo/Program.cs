using Mazes;
using Mazes.Algorithms;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace RecursiveDivisionDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var grid = new Grid(20, 20);
            new RecursiveDivision().On(grid);

            var image = await grid.ToImageAsync();
            image.Save("recursive_division.png", ImageFormat.Png);
        }
    }
}
