using SharpTeX.Renderer.Enums;

namespace SharpTeX.Renderer.Models;

public record RenderedBlock(
    string BlockId,
    string BlockName,
    string Content,
    BlockType BlockType,
    List<RenderedBlock> Children
);