using System.Text;
using CSharpFunctionalExtensions;
using SharpTeX.Extensions;
using SharpTeX.Renderer;
using SharpTeX.Renderer.Models;

namespace SharpTeX.TeXBlock.Document;

public class Document : Block
{
    // TODO add other Document block actions         
    private List<string> DocumentPreContent = new();

    private Document(string blockName)
    {
        BlockName = blockName;
    }

    public static Document CreateDocument()
        => new Document("document");
    
    public Document AddBlock(Block block)
    {
        Children.Add(block);
        return this;
    }

    public Document AddTitle()
    {
        DocumentPreContent.Add(@"\maketitle");
        return this;
    }
    
    protected override Result<RenderedBlock> RenderContent(IRenderer renderer, RenderedBlock block)
    {
        var preContent = string.Join(Environment.NewLine, DocumentPreContent);
        renderer.AddToBlock(block, preContent);
        // TODO: Add sections

        var renderedContent = RenderChildren(renderer, block);

        // TODO: Add bibliography
        return renderedContent
            .Map(content => renderer.AddToBlock(block, content));
    }


}