using CodeSharp.Executor.Contracts.Shared;
using ErrorOr;

namespace CodeSharp.Executor.Common.Extensions;

public static class ResultsExtensions
{
    public static IResult ErrorOrResult<T>(this IResultExtensions _, ErrorOr<T> result)
    {
        if (!result.IsError)
        {
            return Results.Ok(result.Value);
        }
        
        var errorMessages = result
            .Errors
            .Select(error => new ApplicationError(error.Code, error.Description));
            
        return Results.BadRequest(errorMessages);

    }
}