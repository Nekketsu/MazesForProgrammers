using System;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Mazes
{
    public class Mask
    {
        public int Rows { get; }
        public int Columns { get; }

        bool[,] bits;

        Random random;

        public Mask(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;

            bits = new bool[rows, columns];
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < columns; j++)
                {
                    bits[i, j] = true;
                }
            }

            random = new Random();
        }

        public static Mask FromTxt(string file)
        {
            var lines = File.ReadLines(file)
                .Select(line => line.Trim())
                .TakeWhile(line => !string.IsNullOrEmpty(line))
                .ToArray();

            if (!lines.Any())
            {
                return null;
            }

            var rows = lines.Count();
            var columns = lines.First().Count();

            if (lines.Any(line => line.Length != columns))
            {
                return null;
            }

            var mask = new Mask(rows, columns);

            for (var row = 0; row < rows; row++)
            {
                for (var column = 0; column < columns; column++)
                {
                    mask[row, column] = lines[row][column] != 'X';
                }
            }

            return mask;
        }

        public static Mask FromImage(string file)
        {
            var bitmap = new Bitmap(file);
            var mask = new Mask(bitmap.Height, bitmap.Width);

            for (var row = 0; row < mask.Rows; row++)
            {
                for (var column = 0; column < mask.Columns; column++)
                {
                    mask[row, column] = bitmap.GetPixel(column, row).ToArgb() != Color.Black.ToArgb();
                }
            }

            return mask;
        }

        public bool this[int row, int column]
        {
            get => row >= 0 && row < Rows && column >= 0 && column < Columns ? bits[row, column] : false;
            set => bits[row, column] = value;
        }

        public int Count()
        {
            var count = bits.Cast<bool>().Count(b => b);

            return count;
        }

        public (int row, int column) RandomLocation()
        {
            int row;
            int column;

            do
            {
                row = random.Next(Rows);
                column = random.Next(Columns);
            } while (!bits[row, column]);

            return (row, column);
        }
    }
}
