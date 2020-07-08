using Mazes;
using Mazes.Algorithms;
using System.Threading.Tasks;

namespace AldousBroderDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var grid = new Grid(20, 20);
            new AldousBroder().On(grid);

            var image = await grid.ToImageAsync();
            image.Save("AldousBroder.png");
        }
    }
}
