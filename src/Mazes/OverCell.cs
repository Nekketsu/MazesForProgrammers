using System.Collections.Generic;
using System.Linq;

namespace Mazes
{
    public class OverCell : WeaveCell
    {
        WeaveGrid grid;

        public OverCell(int row, int column, WeaveGrid grid) : base(row, column)
        {
            this.grid = grid;
        }

        public override IEnumerable<Cell> Neighbors()
        {
            var neighbors = base.Neighbors().ToList();
            if (CanTunnelNorth)
            {
                neighbors.Add(North.North);
            }
            if (CanTunnelSouth)
            {
                neighbors.Add(South.South);
            }
            if (CanTunnelEast)
            {
                neighbors.Add(East.East);
            }
            if (CanTunnelWest)
            {
                neighbors.Add(West.West);
            }

            return neighbors.AsEnumerable();
        }

        public override void Link(Cell cell, bool bidirectional = true)
        {
            Cell neighboor = null;

            if (North != null && North == cell.South)
            {
                neighboor = North;
            }
            else if (South != null && South == cell.North)
            {
                neighboor = South;
            }
            else if (East != null && East == cell.West)
            {
                neighboor = East;
            }
            else if (West != null && West == cell.East)
            {
                neighboor = West;
            }

            if (neighboor != null)
            {
                grid.TunnelUnder((OverCell)neighboor);
            }
            else
            {
                base.Link(cell, bidirectional);
            }
        }

        public bool CanTunnelNorth =>
            North?.North != null &&
            ((WeaveCell)North).HorizontalPassage;

        public bool CanTunnelSouth =>
            South?.South != null &&
            ((WeaveCell)South).HorizontalPassage;

        public bool CanTunnelEast =>
            East?.East != null &&
            ((WeaveCell)East).VerticalPassage;

        public bool CanTunnelWest =>
            West?.West != null &&
            ((WeaveCell)West).VerticalPassage;


        public override bool HorizontalPassage =>
            Linked(East) && Linked(West) &&
            !Linked(North) && !Linked(South);

        public override bool VerticalPassage =>
            Linked(North) && Linked(South) &&
            !Linked(East) && !Linked(West);
    }
}
