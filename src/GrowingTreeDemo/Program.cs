using Mazes;
using Mazes.Algorithms;
using Mazes.Extensions;
using System;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;

namespace GrowingTreeDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var random = new Random();
            var growingTree = new GrowingTree();

            var grid = new Grid(20, 20);
            growingTree.On(grid, list => list.Sample());
            (await grid.ToImageAsync()).Save("growing-tree-random.png", ImageFormat.Png);

            grid = new Grid(20, 20);
            growingTree.On(grid, list => list.Last());
            (await grid.ToImageAsync()).Save("growing-tree-last.png", ImageFormat.Png);

            grid = new Grid(20, 20);
            growingTree.On(grid, list => random.Next(2) == 0 ? list.Last() : list.Sample());
            (await grid.ToImageAsync()).Save("growing-tree-mix.png", ImageFormat.Png);
        }
    }
}
