using System.Collections.ObjectModel;
using System.Text;
using CSharpFunctionalExtensions;
using SharpTeX.Extensions;
using SharpTeX.Renderer;
using SharpTeX.Renderer.Models;

namespace SharpTeX.TeXBlock;

public abstract class Block : IRenderable
{
    protected List<Block> Children { get; } = new();

    public string BlockName { get; protected init; } = null!;

    protected abstract Result<RenderedBlock> RenderContent(IRenderer renderer, RenderedBlock block);

    public ReadOnlyCollection<Block> GetChildren() => Children.AsReadOnly();

    public virtual Result<RenderedBlock> Render(IRenderer renderer)
    {
        var block = renderer.AddNamedBlock(BlockName);
        return RenderContent(renderer, block);
    }
    
    protected Result<List<RenderedBlock>> RenderChildren(IRenderer renderer)
    {
        return Children
            .Select(child => child.Render(renderer))
            .Collect()
            .Map(children => children.ToList());
    }
}