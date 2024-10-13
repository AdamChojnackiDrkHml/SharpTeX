using System.Text;
using SharpTeX.Extensions;

namespace SharpTeX.TeXBlock;

public abstract class Block
{
    protected List<Block> Children { get; } = new();

    protected string BlockName;
    
    protected abstract string RenderContent();

    public string Render()
    {
        var builder = new StringBuilder();
        builder.AppendLine($@"\begin{{{BlockName}}}");
        builder.AppendLine(RenderContent().IndentEachLine());
        builder.Append($@"\end{{{BlockName}}}");

        return builder.ToString();
    }
    
    protected string RenderChildren()
    {
        var builder = new StringBuilder();
        foreach (var child in Children)
        {
            builder.Append(child.Render());
        }

        return builder.ToString();
    }
}