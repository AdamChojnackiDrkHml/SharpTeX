using FluentAssert;
using FluentAssertions;
using Moq;
using SharpTeX.Renderer;
using SharpTeX.Renderer.Models;
using TBlock = SharpTeX.TeXBlock.SimpleBlock.TextBlock.TextBlock;
namespace SharpTeX.Test.TeXBlock.SimpleBlock.TextBlock;

public class TextBlockTests
{
    [Fact]
    public void CreateTextBlock_VariousContent_ReturnsCorrectTextBlock()
    {
        // Arrange
        var content = "Hello, World!";
        var expectedContent = "Hello, World!";
        
        // Act
        var result = TBlock.CreateTextBlock(content);
        
        // Assert
        result.Content.Should().BeEquivalentTo(expectedContent);
    }

    [Fact]
    public void CreateTextBlock_Parameterless_ReturnsCorrectTextBlock()
    {
        // Arrange
        const string expectedContent = "";
        
        // Act
        var result = TBlock.CreateTextBlock();
        
        // Assert
        result.Content.Should().BeEquivalentTo(expectedContent);
    }
    
    [Theory]
    [MemberData(nameof(AppendTestCases))]
    public void Append_VariousContent_ReturnsCorrectTextBlock(
        TBlock textBlock,
        string addedContent,
        string separator, 
        string expectedContent
    )
    {
        // Act
        var result = textBlock.Append(addedContent, separator);
        
        // Assert
        result.ShouldBeSameInstanceAs(textBlock);
        result.Content.Should().BeEquivalentTo(expectedContent);
    }
    
    [Fact]
    public void Append_DefaultSeparator_ReturnsCorrectTextBlock()
    {
        // Arrange
        var textBlock = TBlock.CreateTextBlock("My Name is");
        const string addedContent = "Gyobu";
        const string expectedContent = "My Name isGyobu";
        
        // Act
        var result = textBlock.Append(addedContent);
        
        // Assert
        result.Content.Should().BeEquivalentTo(expectedContent);
    }
    
    [Theory]
    [MemberData(nameof(RenderTestCases))]
    public void Render_DifferentTextBlocks_ReturnsCorrectRenderedBlock(
        TBlock textBlock
    )
    {
        // Arrange
        var renderer = new Mock<IRenderer>();
        
        // Act
        var result = textBlock.Render(renderer.Object);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        renderer.Verify(x => x.AddSimpleBlock(It.IsAny<string?>()), Times.Once);
        renderer.Verify(x => x.AddToBlock(result.Value, textBlock.Content), Times.Once);
        renderer.VerifyNoOtherCalls();
    }

    public static List<object[]> AppendTestCases =
    [
        new object[]
        {
            TBlock.CreateTextBlock("Hello"),
            "World!",
            " ",
            "Hello World!"
        },    
        new object[]
        {
            TBlock.CreateTextBlock(),
            "Emptiness",
            Environment.NewLine,
            $"{Environment.NewLine}Emptiness"
        }
    ];

    public static List<object[]> RenderTestCases =
    [
        new object[] {TBlock.CreateTextBlock("This is text block")},
        new object[] {TBlock.CreateTextBlock()}
    ];


}