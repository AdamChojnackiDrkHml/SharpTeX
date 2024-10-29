using FluentAssert;
using FluentAssertions;
using FluentAssertions.CSharpFunctionalExtensions;
using Moq;
using SharpTeX.Renderer;
using SharpTeX.Renderer.Models;
using SharpTeX.TeXBlock.SimpleBlock;
using SharpTeX.TeXBlock.SimpleBlock.TextBlock;
using static SharpTeX.Extensions.StringExtensions;
using static SharpTeX.Test.TeXBlock.ListBlock.ListBlockTestsData;

namespace SharpTeX.Test.TeXBlock.ListBlock;

public class ListBlockTests
{
    // Happy Day tests
    
    [Theory]
    [MemberData(nameof(SimpleBlocks), MemberType = typeof(ListBlockTestsData))]
    public void AddItem_CorrectBlock_ReturnsCorrectBlock(SharpTeX.TeXBlock.SimpleBlock.SimpleBlock item)
    {
        // Arrange
        var listBlock = SharpTeX.TeXBlock.ListBlock.ListBlock.NewItemize();
        
        // Act
        var listBlockRef = listBlock.AddItem(item);
        
        // Assert
        listBlockRef.ShouldBeSameInstanceAs(listBlock);
        listBlockRef.Items.Should().BeEquivalentTo([item]);
    }
    
    [Fact]
    public void AddItems_CorrectBlocks_ReturnsCorrectBlock()
    {
        // Arrange
        var listBlock = SharpTeX.TeXBlock.ListBlock.ListBlock.NewItemize();
        var items = new List<SharpTeX.TeXBlock.SimpleBlock.SimpleBlock>
        {
            TextBlock.CreateTextBlock("Text1"),
            TextBlock.CreateTextBlock("Text2")
        };
        
        // Act
        var listBlockRef = listBlock.AddItems(items);
        
        // Assert
        listBlockRef.ShouldBeSameInstanceAs(listBlock);
        listBlockRef.Items.Should().BeEquivalentTo(items);
    }
    
    [Theory]
    [MemberData(nameof(Strings), MemberType = typeof(ListBlockTestsData))]
    public void AddItem_Strings_ReturnsCorrectBlock(string item, SharpTeX.TeXBlock.SimpleBlock.SimpleBlock expected)
    {
        // Arrange
        var listBlock = SharpTeX.TeXBlock.ListBlock.ListBlock.NewItemize();
        
        // Act
        var listBlockRef = listBlock.AddItem(item);
        
        // Assert
        listBlockRef.ShouldBeSameInstanceAs(listBlock);
        listBlockRef.Items.Should().HaveCount(1);
        listBlockRef.Items[0].Should().BeOfType(expected.GetType());
        listBlockRef.Items[0].GetContent().Should().Be(expected.GetContent());
    }
    
    [Fact]
    public void AddItems_Strings_ReturnsCorrectBlock()
    {
        // Arrange
        var listBlock = SharpTeX.TeXBlock.ListBlock.ListBlock.NewItemize();
        var items = new List<string> {"Text1", "Text2"};
        var expected = items
            .Select(item => TextBlock.CreateTextBlock(item));
        
        // Act
        var listBlockRef = listBlock.AddItems(items);
        
        // Assert
        listBlockRef.ShouldBeSameInstanceAs(listBlock);
        listBlockRef.Items.Should().HaveCount(2);
        listBlockRef.Items.Should().BeEquivalentTo(expected);
    }
    
    [Theory]
    [MemberData(nameof(DifferentObjectsWithMappers), MemberType = typeof(ListBlockTestsData))]
    public void AddItem_DifferentObjectsWithMappers_ReturnsCorrectBlock<T>(
        T item,
        Func<T, SharpTeX.TeXBlock.SimpleBlock.SimpleBlock> textMapper,
        SharpTeX.TeXBlock.SimpleBlock.SimpleBlock expected)
    {
        if (expected == null)
            throw new ArgumentNullException(nameof(expected));


        // Arrange
        var listBlock = SharpTeX.TeXBlock.ListBlock.ListBlock.NewItemize();
        
        // Act
        var listBlockRef = listBlock.AddItem(item, textMapper);
        
        // Assert
        listBlockRef.ShouldBeSameInstanceAs(listBlock);
        listBlockRef.Items.Should().HaveCount(1);
        listBlockRef.Items[0].Should().BeOfType(expected.GetType());
        listBlockRef.Items[0].GetContent().Should().Be(expected.GetContent());
    }
    
    [Fact]
    public void AddItems_DifferentObjectsWithMappers_ReturnsCorrectBlock()
    {
        // Arrange
        var listBlock = SharpTeX.TeXBlock.ListBlock.ListBlock.NewItemize();
        var items = new List<object> {1234, DateTime.Today.Date, new {Name = "Adam", Index = 1234}};

        var expected = items
            .Select(item => TextBlock.CreateTextBlock(item.ToString()));
        
        // Act
        var listBlockRef = listBlock.AddItems(items, item => TextBlock.CreateTextBlock(item.ToString()));
        
        // Assert
        listBlockRef.ShouldBeSameInstanceAs(listBlock);
        listBlockRef.Items.Should().HaveCount(3);
        listBlockRef.Items.Should().BeEquivalentTo(expected);
    }
    
    // Fail tests
    
    [Fact]
    public void AddItem_MapperReturnsError_ThrowsArgumentNullException()
    {
        // Arrange
        var listBlock = SharpTeX.TeXBlock.ListBlock.ListBlock.NewItemize();
        var mapper = new Func<object, SharpTeX.TeXBlock.SimpleBlock.SimpleBlock>(o => null);
        
        // Act
        Action act = () => listBlock.AddItem(1234, mapper);
        
        // Assert
        act.Should().Throw<ArgumentNullException>("Argument Cannot Be Null");
    }
    
    [Fact]
    public void AddItems_OneOfMappersReturnNull_ThrowsArgumentNullException()
    {
        // Arrange
        var listBlock = SharpTeX.TeXBlock.ListBlock.ListBlock.NewItemize();
        var mapper = new Func<object, SharpTeX.TeXBlock.SimpleBlock.SimpleBlock>(o => null);
        
        // Act
        Action act = () => listBlock.AddItems(new List<object> {1234}, mapper);
        
        // Assert
        act.Should().Throw<ArgumentNullException>("Argument Cannot Be Null");
    }

    [Theory]
    [MemberData(nameof(ListBlockInstances), MemberType = typeof(ListBlockTestsData))]
    public void Render_EmptyItemizeBlock_RendererIsNotInvoked(
        Func<SharpTeX.TeXBlock.ListBlock.ListBlock> listConstructor, 
        string listType
    )
    {
        // Arrange
        var renderer = new Mock<IRenderer>();
        var list = listConstructor();
        
        // Act
        var res = list.Render(renderer.Object);
        
        // Assert
        res.IsSuccess.Should().BeTrue();
        renderer.Verify(x => x.AddNamedBlock(listType, null), Times.Once);
        renderer.VerifyNoOtherCalls();
    }
    
    [Theory]
    [MemberData(nameof(ListBlockInstances), MemberType = typeof(ListBlockTestsData))]
    public void Render_NonEmptyItems_ReturnsSuccess(
        Func<SharpTeX.TeXBlock.ListBlock.ListBlock> listConstructor,
        string listType
    )
    {
        // Arrange
        var renderer = new Mock<IRenderer>();
        var list = listConstructor();
        list.AddItem("Item 1").AddItem("Item 2");
        
        // Act
        var result = list.Render(renderer.Object);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        renderer.Verify(x => x.AddNamedBlock(listType, It.IsAny<string?>()), Times.Once);
        renderer.Verify(x => x.AddSimpleBlock(null), Times.Exactly(2));
        renderer.Verify(x => x.AddToBlock(It.IsAny<RenderedBlock>(), "Item 1"));
        renderer.Verify(x => x.AddToBlock(It.IsAny<RenderedBlock>(), "Item 2"));
        renderer.Verify(x => x.AddToBlock(It.IsAny<RenderedBlock>(), It.IsAny<RenderedBlock>()), Times.Exactly(2));
        renderer.VerifyNoOtherCalls();
    }
}