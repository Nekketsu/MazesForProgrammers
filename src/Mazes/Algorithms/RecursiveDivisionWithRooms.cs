using System;

namespace Mazes.Algorithms
{
    public class RecursiveDivisionWithRooms : RecursiveDivision
    {
        Random random;

        public RecursiveDivisionWithRooms()
        {
            random = new Random();
        }

        public override bool ShouldCreateRoom(int height, int width)
        {
            return height <= 1 || width <= 1 ||
                   height < 5 && width < 5 && random.Next(4) == 0;
        }
    }
}
