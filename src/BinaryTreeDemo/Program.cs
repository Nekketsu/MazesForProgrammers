using Mazes;
using Mazes.Algorithms;
using System;

namespace BinaryTreeDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var grid = new Grid(4, 4);
            new BinaryTree().On(grid);

            Console.WriteLine(grid);
        }
    }
}
