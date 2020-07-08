using Mazes.Algorithms;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Mazes.Blazor.Pages
{
    public partial class Mazes
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string Image { get; set; }

        public void GenerateMaze()
        {
            var grid = new Grid(Width, Height);

            var sidewinder = new Sidewinder();
            sidewinder.On(grid);

            var bitmap = grid.ToBitmap();
            var memoryStream = new MemoryStream();
            bitmap.Save(memoryStream, ImageFormat.Png);
            var imageBytes = memoryStream.ToArray();
            Image = Convert.ToBase64String(imageBytes);
        }
    }
}
