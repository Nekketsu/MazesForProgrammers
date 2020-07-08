using Mazes;
using Mazes.Algorithms;
using System;

namespace Dijkstra
{
    class Program
    {
        static void Main(string[] args)
        {
            var grid = new DistanceGrid(5, 5);
            new BinaryTree().On(grid);

            var start = grid[0, 0];
            var distances = start.Distances();

            grid.Distances = distances;
            Console.WriteLine(grid);

            Console.WriteLine("path from northwest corner to southwest corner:");
            grid.Distances = distances.PathTo(grid[grid.Rows - 1, 0]);
            Console.WriteLine(grid);
        }
    }
}
