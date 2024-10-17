using CSharpFunctionalExtensions;

namespace SharpTeX.Extensions;

public static class ResultExtensions
{
    public static Result<IEnumerable<T>> Collect<T>(this IEnumerable<Result<T>> results)
    {
        var failures = results
            .Where(result => result.IsFailure)
            .Select(result => result.Error)
            .ToList();

        return failures.Any() 
            ? Result.Failure<IEnumerable<T>>(failures.JoinNonEmpty(Environment.NewLine)) 
            : results.Combine();
    }
}