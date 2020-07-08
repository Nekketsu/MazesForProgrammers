using Mazes;
using Mazes.Algorithms;
using System;

namespace KillingCells
{
    class Program
    {
        static void Main(string[] args)
        {
            var grid = new Grid(5, 5);

            // Orphan the cell in the northwest corner...
            grid[0, 0].East.West = null;
            grid[0, 0].South.North = null;

            // ... and the one in the southeast corner
            grid[4, 4].West.East = null;
            grid[4, 4].North.South = null;

            new RecursiveBacktracker(grid[1, 1]).On(grid);

            Console.WriteLine(grid);
        }
    }
}
