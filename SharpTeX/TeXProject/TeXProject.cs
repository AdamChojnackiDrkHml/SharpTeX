using System.Text;
using CSharpFunctionalExtensions;
using SharpTeX.TeXBlock.Document;

namespace SharpTeX.TeXProject;

public class TeXProject
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
    
    public Result<string> Render()
    {
        if (_document == null)
        {
            return Result.Failure<string>("Document is not set.");
        }
        
        var builder = new StringBuilder();
        
        builder.AppendLine(@$"\documentclass{{{_documentClass}}}");
        builder.AppendLine(@"\usepackage[english]{babel}");
        builder.AppendLine(@"\usepackage[a4paper,top=2cm,bottom=2cm,left=3cm,right=3cm,marginparwidth=1.75cm]{geometry}");

        builder.AppendLine($@"\title{{{Title}}}");
        builder.AppendLine(RenderAuthor());
        builder.AppendLine(_document.Render());
        
        return builder.ToString();
    }
    
    private string RenderAuthor()
    {
        return string.IsNullOrWhiteSpace(Author) ? string.Empty : $@"\author{{{Author}}}";
    }
}