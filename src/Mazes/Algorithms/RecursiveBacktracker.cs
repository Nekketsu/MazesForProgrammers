using Mazes.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Mazes.Algorithms
{
    public class RecursiveBacktracker : IAlgorithm
    {
        public Cell StartAt { get; set; }

        public RecursiveBacktracker() : this(null) { }

        public RecursiveBacktracker(Cell startAt)
        {
            StartAt = startAt;
        }

        public IGrid On(IGrid grid)
        {
            var startAt = StartAt ?? grid.RandomCell();

            var stack = new Stack<Cell>();
            stack.Push(startAt);

            while (stack.Any())
            {
                var current = stack.Peek();
                var neighbors = current.Neighbors().Where(n => !n.Links.Any());

                if (!neighbors.Any())
                {
                    stack.Pop();
                }
                else
                {
                    var neighbor = neighbors.Sample();
                    current.Link(neighbor);
                    stack.Push(neighbor);
                }
            }

            return grid;
        }
    }
}
