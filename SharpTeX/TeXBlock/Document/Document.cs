using System.Text;
using SharpTeX.Extensions;

namespace SharpTeX.TeXBlock.Document;

public class Document : Block
{
    // TODO add other Document block actions         


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
    
    protected override string RenderContent()
    {
        var lines = new List<string>();
        
        lines.Add(@"\maketitle");
        
        
        var preContent = string.Join(Environment.NewLine, lines);
        // TODO: Add sections

        var content = preContent.JoinIfNotEmpty(RenderChildren(), Environment.NewLine);
        
        // TODO: Add bibliography

        return content;
    }


}