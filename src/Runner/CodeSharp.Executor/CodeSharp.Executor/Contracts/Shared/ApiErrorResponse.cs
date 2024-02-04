namespace CodeSharp.Executor.Contracts.Shared;

public class ApiErrorResponse
{
    public IReadOnlyCollection<ApplicationError> Errors { get; }

    public ApiErrorResponse(IReadOnlyCollection<ApplicationError> errors)
    {
        Errors = errors;
    }
}