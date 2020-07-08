using System.Collections.Generic;
using System.Linq;

namespace Mazes
{
    public class Cell
    {
        public int Row { get; }
        public int Column { get; }

        public Cell North { get; set; }
        public Cell South { get; set; }
        public Cell East { get; set; }
        public Cell West { get; set; }

        public HashSet<Cell> Links { get; }

        public Cell(int row, int column)
        {
            Row = row;
            Column = column;

            Links = new HashSet<Cell>();
        }

        public virtual void Link(Cell cell, bool bidirectional = true)
        {
            Links.Add(cell);
            if (bidirectional)
            {
                cell.Link(this, false);
            }
        }

        public void Unlink(Cell cell, bool bidirectional = true)
        {
            Links.Remove(cell);
            if (bidirectional)
            {
                cell.Unlink(this, false);
            }
        }

        public bool Linked(Cell cell)
        {
            return Links.Contains(cell);
        }

        public virtual IEnumerable<Cell> Neighbors()
        {
            var neighbors = new[] { North, South, East, West };

            return neighbors.Where(neighboor => neighboor != null);
        }

        public virtual Distances Distances()
        {
            var distances = new Distances(this);

            var frontier = new List<Cell> { this };

            while (frontier.Any())
            {
                var newFrontier = new List<Cell>();

                foreach (var cell in frontier)
                {
                    foreach (var linked in cell.Links)
                    {
                        if (distances[linked] >= 0)
                        {
                            continue;
                        }
                        distances[linked] = distances[cell] + 1;
                        newFrontier.Add(linked);
                    }
                }

                frontier = newFrontier;
            }

            return distances;
        }
    }
}
