using CSharpFunctionalExtensions;
using SharpTeX.Renderer.Models;

namespace SharpTeX.Renderer;

public interface IRenderable
{
    Result<RenderedBlock> Render(IRenderer renderer);
}