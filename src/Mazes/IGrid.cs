using Mazes.Renderers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mazes
{
    public interface IGrid
    {
        int Size { get; }
        IEnumerable<Cell> EachCell();
        IEnumerable<Cell[]> EachRow();
        Cell RandomCell();
        Task RenderAsync(IRenderer renderer, int cellSize = 10, int inset = 0);
        void Braid(float p = 1f);
    }
}
