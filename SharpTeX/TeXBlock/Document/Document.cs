using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks.Dataflow;
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
        block = renderer.AddToBlock(block, preContent);
        // TODO: Add sections
        // TODO: Add bibliography

        return RenderChildren(renderer)
            .Map(children => children
                .Aggregate(block, renderer.AddToBlock));
    }


}