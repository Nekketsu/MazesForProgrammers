using Mazes;
using Mazes.Algorithms;
using System;

namespace SimpleMask
{
    class Program
    {
        static void Main(string[] args)
        {
            var mask = new Mask(5, 5);

            mask[0, 0] = false;
            mask[2, 2] = false;
            mask[4, 4] = false;

            var grid = new MaskedGrid(mask);
            new RecursiveBacktracker().On(grid);

            Console.WriteLine(grid);
        }
    }
}
