using System.Collections.ObjectModel;
using System.Text;
using CSharpFunctionalExtensions;
using SharpTeX.Extensions;
using SharpTeX.Renderer;
using SharpTeX.Renderer.Models;

namespace SharpTeX.TeXBlock.Document;

public class Document : Block
{
    // TODO add other Document block actions         
    private readonly List<string> _documentPreContent = [];

    private Document()
    {
        BlockName = "document";
    }

    public static Document CreateDocument() => new();
    
    public Document AddBlock(Block block)
    {
        Children.Add(block);
        return this;
    }

    public Document AddTitle()
    {
        _documentPreContent.Add(@"\maketitle");
        return this;
    }
    
    public ReadOnlyCollection<string> GetDocumentPreContent() 
        => _documentPreContent.AsReadOnly();

    protected override Result<RenderedBlock> RenderContent(IRenderer renderer, RenderedBlock block)
    {
        var preContent = string.Join(Environment.NewLine, _documentPreContent);
        renderer.AddToBlock(block, preContent);
        // TODO: Add sections
        // TODO: Add bibliography
        
        
        return RenderChildren(renderer)
            .Tap(children => children
                .ForEach(child => renderer.AddToBlock(block, child)))
            .Map(_ => block);
    }


}