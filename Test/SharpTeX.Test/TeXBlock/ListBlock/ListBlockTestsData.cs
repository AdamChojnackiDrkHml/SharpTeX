using SharpTeX.TeXBlock.SimpleBlock;
using SharpTeX.TeXBlock.SimpleBlock.TextBlock;

namespace SharpTeX.Test.TeXBlock.ListBlock;

public static class ListBlockTestsData
{
    public readonly static List<object[]> SimpleBlocks =
    [
        new object[] {TextBlock.CreateTextBlock("Text1")},
        new object[] {TextBlock.CreateTextBlock()}
    ];
    
    public readonly static List<object?[]> Strings =
    [
        new object[] {"Text1", TextBlock.CreateTextBlock("Text1")},
        new object[] {"Text2", TextBlock.CreateTextBlock("Text2")},
    ];

    public readonly static List<object?[]> DifferentObjectsWithMappers =
    [
        new object[]
        {
            1234, 
            new Func<int, SharpTeX.TeXBlock.SimpleBlock.SimpleBlock>(i => TextBlock.CreateTextBlock(i.ToString())), 
            TextBlock.CreateTextBlock("1234")
        },
        new object[]
        {
            DateTime.Today.Date,
            new Func<DateTime, SharpTeX.TeXBlock.SimpleBlock.SimpleBlock>(d => TextBlock.CreateTextBlock(d.ToString("yyyy-MM-dd"))),
            TextBlock.CreateTextBlock(DateTime.Today.Date.ToString("yyyy-MM-dd"))
        }        
    ];
    
    public readonly static List<object[]> ListBlockInstances =
    [
        new object[] {() => SharpTeX.TeXBlock.ListBlock.ListBlock.NewItemize(), "itemize"},
        new object[] {() => SharpTeX.TeXBlock.ListBlock.ListBlock.NewEnumerate(), "enumerate"}
    ];
}