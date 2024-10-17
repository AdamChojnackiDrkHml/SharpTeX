namespace SharpTeX.Renderer.Models;

public record RenderedBlock(
    string BlockId,
    string BlockName,
    string Content
);