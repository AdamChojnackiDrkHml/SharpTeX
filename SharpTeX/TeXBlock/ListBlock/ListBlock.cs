using System.Text;
using CSharpFunctionalExtensions;
using SharpTeX.Extensions;
using SharpTeX.Renderer;
using SharpTeX.Renderer.Models;
using static SharpTeX.Extensions.TextBlockExtensions;

namespace SharpTeX.TeXBlock.ListBlock;

public class ListBlock : Block
{
    public readonly List<SimpleBlock.SimpleBlock> Items = new();
    
    private ListBlock() {}
    
    public static ListBlock NewItemize()
    {
        return new ListBlock { BlockName = "itemize" };
    }
    
    public static ListBlock NewEnumerate()
    {
        return new ListBlock { BlockName = "enumerate" };
    }
    
    public ListBlock AddItem(SimpleBlock.SimpleBlock item)
    {
        if (item is null)
        {
            throw new ArgumentNullException(nameof(item), "Argument Cannot Be Null");
        }
        
        Items.Add(item);
        return this;
    }
    
    public ListBlock AddItems(IEnumerable<SimpleBlock.SimpleBlock> items)
    {
        if (items is null)
        {
            throw new ArgumentNullException(nameof(items), "Argument Cannot Be Null");
        }
            
        if (items.Contains(null))
        {
            throw new ArgumentNullException(nameof(items), "Argument Cannot Contain Null");
        }
        
        Items.AddRange(items);
        return this;
    }
    
    public ListBlock AddItem(string item) 
        => AddItem(item.ToTextBlock());

    public ListBlock AddItems(IEnumerable<string> items) 
        => AddItems(items.Select(item => item.ToTextBlock()));

    public ListBlock AddItem<T>(T item, Func<T, SimpleBlock.SimpleBlock> textMapper)
    {
        return AddItem(textMapper(item));
    }
    
    public ListBlock AddItems<T>(IEnumerable<T> items, Func<T, SimpleBlock.SimpleBlock> textMapper)
    {
        return AddItems(items.Select(textMapper));
    }
    
    protected override Result<RenderedBlock> RenderContent(IRenderer renderer, RenderedBlock block)
    {
        Items.ForEach(item => renderer.AddToBlock(block, item.GetContent()));

        return block;
    }
}