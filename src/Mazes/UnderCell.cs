namespace Mazes
{
    public class UnderCell : WeaveCell
    {
        public UnderCell(OverCell overCell) : base(overCell.Row, overCell.Column)
        {
            if (overCell.HorizontalPassage)
            {
                North = overCell.North;
                overCell.North.South = this;
                South = overCell.South;
                overCell.South.North = this;

                Link(North);
                Link(South);
            }
            else
            {
                East = overCell.East;
                overCell.East.West = this;
                West = overCell.West;
                overCell.West.East = this;

                Link(East);
                Link(West);
            }
        }

        public override bool HorizontalPassage => East != null || West != null;

        public override bool VerticalPassage => North != null || South != null;
    }
}
