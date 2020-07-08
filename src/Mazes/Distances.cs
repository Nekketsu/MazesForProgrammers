using System.Collections.Generic;

namespace Mazes
{
    public class Distances
    {
        public Cell Root { get; set; }
        Dictionary<Cell, int> cells;

        public Distances(Cell root)
        {
            Root = root;
            cells = new Dictionary<Cell, int>();
            cells.Add(root, 0);
        }

        public int this[Cell cell]
        {
            get => cells.TryGetValue(cell, out var distance) ? distance : -1;
            set => cells[cell] = value;
        }

        public IEnumerable<Cell> Cells => cells.Keys;

        public Distances PathTo(Cell goal)
        {
            var current = goal;

            var breadcrumbs = new Distances(Root);
            breadcrumbs[current] = cells[current];

            while (current != Root)
            {
                foreach (var neighbor in current.Links)
                {
                    if (cells[neighbor] < cells[current])
                    {
                        breadcrumbs[neighbor] = cells[neighbor];
                        current = neighbor;
                        break;
                    }
                }
            }

            return breadcrumbs;
        }

        public (Cell, int) Max()
        {
            var maxCell = Root;
            var maxDistance = 0;

            foreach (var cell in cells)
            {
                if (cell.Value > maxDistance)
                {
                    maxCell = cell.Key;
                    maxDistance = cell.Value;
                }
            }

            return (maxCell, maxDistance);
        }
    }
}
