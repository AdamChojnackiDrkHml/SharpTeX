using CSharpFunctionalExtensions;
using SharpTeX.Renderer;
using SharpTeX.Renderer.Models;

namespace SharpTeX.TeXBlock.SimpleBlock.TextBlock;

public class TextBlock : SimpleBlock
{
    public string Content { get; private set; } = null!;

    private TextBlock() {}

    public static TextBlock CreateTextBlock(string content = "")
    {
        var block = new TextBlock
        {
            Content = content
        };
        
        return block;
    }
    
    protected override Result<RenderedBlock> RenderContent(IRenderer renderer, RenderedBlock block)
    {
        return renderer.AddToBlock(block, Content);
    }

    public override Result<RenderedBlock> Render(IRenderer renderer)
    {
        var block = renderer.AddSimpleBlock();
        return RenderContent(renderer, block);
    }

    public override string GetContent() => Content;
    
    public override TextBlock SetContent(string content)
    {
        var newBlock = CreateTextBlock(content);
        return newBlock;
    }

    public TextBlock Append(string content, string separator = "")
    {
        this.Content += separator + content;
        return this;
    }
    
}