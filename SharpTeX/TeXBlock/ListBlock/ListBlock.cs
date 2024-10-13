using System.Text;
using SharpTeX.Extensions;

namespace SharpTeX.TeXBlock.ListBlock;

public class ListBlock : Block
{
    private List<string> _items = new();
    
    private ListBlock() {}
    
    public static ListBlock NewItemize()
    {
        return new ListBlock { BlockName = "itemize" };
    }
    
    public static ListBlock NewEnumerate()
    {
        return new ListBlock { BlockName = "enumerate" };
    }
    
    public ListBlock AddItem(string item)
    {
        _items.Add(item);
        return this;
    }
    
    public ListBlock AddItems(IEnumerable<string> items)
    {
        _items.AddRange(items);
        return this;
    }
    
    protected override string RenderContent()
    {
        return string.Join(Environment.NewLine, _items.Select(item => $@"\item {item}"));
    }
}