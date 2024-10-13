using System.Text;

namespace SharpTeX.Extensions;

internal static class StringExtensions
{
    public static string NewLine(this string str) 
        => str + Environment.NewLine;
    
    public static string Indent(this string str, int count = 1)
        => new string('\t', count) + str;

    public static string IndentEachLine(this string str)
    {
        if (string.IsNullOrWhiteSpace(str))
        {
            return str;
        }
        
        return str.Split(Environment.NewLine)
            .Select(line => $"\t{line}")
            .Aggregate((curr, next) => $"{curr}{Environment.NewLine}{next}");
    }
    
    public static string JoinIfNotEmpty(this string str, string str2, string separator)
    {
        return string.IsNullOrWhiteSpace(str2) ? str : $"{str}{separator}{str2}";
    }
}