using System.Threading.Tasks;

namespace Mazes.Renderers
{
    public interface IRenderer
    {
        Task<IGraphics> CreateAsync(int width, int height);
    }
}
