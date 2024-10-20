using System.Text;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using SharpTeX.Renderer;
using SharpTeX.Renderer.Models;
using SharpTeX.TeXBlock;
using SharpTeX.TeXBlock.Document;
using SharpTeX.TeXBlock.SimpleBlock.TextBlock;

namespace SharpTeX.TeXProject;

public class TeXProject : IRenderable
{
    private string _documentClass = "article";

    private string Title;
    private string? Author;
    
    private Document? _document;
    
    private TeXProject(string title, string? author)
    {
        Title = title;
        Author = author;
    }

    public static TeXProject CreateTeXProject(string title, string? author = null)
        => new TeXProject(title, author);

    public TeXProject AddAuthor(string author)
    {
        Author = author;
        return this;
    }
    
    public TeXProject SetDocumentClass(string documentClass)
    {
        _documentClass = documentClass;
        return this;
    }
    
    public TeXProject AddDocument(Document document)
    {
        _document = document;
        return this;
    }
    
    public Result<RenderedBlock> Render(IRenderer renderer)
    {
        if (_document == null)
        {
            renderer.LogFailure("Document is not set.");
        }

        var projectBlock = renderer.AddSimpleBlock();
        
        var textBlock = TextBlock.CreateTextBlock(@$"\documentclass{{{_documentClass}}}")
            .Append(@"\usepackage[utf8]{inputenc}", Environment.NewLine)
            .Append(@"\usepackage[english]{babel}", Environment.NewLine)
            .Append(@"\usepackage[a4paper,top=2cm,bottom=2cm,left=3cm,right=3cm,marginparwidth=1.75cm]{geometry}", Environment.NewLine)
            .Append($@"\title{{{Title}}}", Environment.NewLine)
            .Append(RenderAuthor(), Environment.NewLine);

        var projectBlockRenderResult = textBlock.Render(renderer)
            .Map(content => renderer.AddToBlock(projectBlock, content))
            .Bind(_ => _document!.Render(renderer))
            .Map(documentRender => renderer.AddToBlock(projectBlock, documentRender));

        return projectBlockRenderResult;
    }
    
    private string RenderAuthor()
    {
        return string.IsNullOrWhiteSpace(Author) ? string.Empty : $@"\author{{{Author}}}";
    }
}