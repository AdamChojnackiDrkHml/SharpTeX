using SharpTeX.TeXBlock.SimpleBlock;
using SharpTeX.TeXBlock.SimpleBlock.TextBlock;

namespace SharpTeX.Test.TeXBlock.ListBlock;

public static class ListBlockTestsData
{
    public readonly static List<object[]> SimpleBlocks =
    [
        new object[] {new TextBlock("Text1")},
        new object[] {new TextBlock()}
    ];
    
    public readonly static List<object?[]> Strings =
    [
        new object[] {"Text1", new TextBlock("Text1")},
        new object[] {"Text2", new TextBlock("Text2")},
    ];

    public readonly static List<object?[]> DifferentObjectsWithMappers =
    [
        new object[]
        {
            1234, 
            new Func<int, SimpleBlock>(i => new TextBlock(i.ToString())), 
            new TextBlock("1234")
        },
        new object[]
        {
            DateTime.Today.Date,
            new Func<DateTime, SimpleBlock>(d => new TextBlock(d.ToString("yyyy-MM-dd"))),
            new TextBlock(DateTime.Today.Date.ToString("yyyy-MM-dd"))
        }        
    ];
    
    public readonly static List<object[]> ListBlockInstances =
    [
        new object[] {() => SharpTeX.TeXBlock.ListBlock.ListBlock.NewItemize(), "itemize"},
        new object[] {() => SharpTeX.TeXBlock.ListBlock.ListBlock.NewEnumerate(), "enumerate"}
    ];
}