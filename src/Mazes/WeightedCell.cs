using System.Collections.Generic;
using System.Linq;

namespace Mazes
{
    public class WeightedCell : Cell
    {
        public int Weight { get; set; }

        public WeightedCell(int row, int column) : base(row, column)
        {
            Weight = 1;
        }

        public override Distances Distances()
        {
            var weights = new Distances(this);
            var pending = new List<WeightedCell> { this };

            while (pending.Any())
            {
                var cell = pending.OrderBy(c => c.Weight).First();
                pending.Remove(cell);

                foreach (var neighbor in cell.Links.Cast<WeightedCell>())
                {
                    var totalWeight = weights[cell] + neighbor.Weight;

                    if (weights[neighbor] == -1 || totalWeight < weights[neighbor])
                    {
                        pending.Add(neighbor);
                        weights[neighbor] = totalWeight;
                    }
                }
            }

            return weights;
        }
    }
}
