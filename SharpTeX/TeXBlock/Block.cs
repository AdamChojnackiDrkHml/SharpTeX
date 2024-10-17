using System.Text;
using CSharpFunctionalExtensions;
using SharpTeX.Extensions;
using SharpTeX.Renderer;
using SharpTeX.Renderer.Models;

namespace SharpTeX.TeXBlock;

public abstract class Block : IRenderable
{
    protected List<Block> Children { get; } = new();

    protected string BlockName;
    
    protected abstract Result<RenderedBlock> RenderContent(IRenderer renderer, RenderedBlock block);

    public Result<RenderedBlock> Render(IRenderer renderer)
    {
        var block = renderer.AddNamedBlock(BlockName);
        return RenderContent(renderer, block);
    }
    
    protected Result<RenderedBlock> RenderChildren(IRenderer renderer, RenderedBlock block)
    {
        var renderedBlocks = Children
            .Select(child => child.Render(renderer))
            .Collect();
        
        if (renderedBlocks.IsFailure)
        {
            return Result.Failure<RenderedBlock>(renderedBlocks.Error);
        }

        foreach (var renderedChild in renderedBlocks.Value)
        {
            renderer.AddToBlock(block, renderedChild);
        }
        
        return Result.Success(block);
    }
}