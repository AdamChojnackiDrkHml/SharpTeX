using CSharpFunctionalExtensions;
using SharpTeX.Renderer;
using SharpTeX.Renderer.Models;

namespace SharpTeX.TeXBlock.SimpleBlock.TextBlock;

public class TextBlock : SimpleBlock
{
    public string Content { get; }
    
    public TextBlock(string? content = null)
    {
        Content = string.IsNullOrEmpty(content) 
            ? string.Empty 
            : content;
    }
    
    protected override Result<RenderedBlock> RenderContent(IRenderer renderer, RenderedBlock block)
    {
        return renderer.AddToBlock(block, Content);
    }

    public new Result<RenderedBlock> Render(IRenderer renderer)
    {
        var block = renderer.AddSimpleBlock();
        return RenderContent(renderer, block);
    }

    public override string GetContent()
    {
        return Content;
    }
    
    public TextBlock Append(string content, string separator = "")
    {
        return new TextBlock(Content + separator + content);
    }
}