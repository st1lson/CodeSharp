namespace CodeSharp.Executor.Contracts.Shared;

public record ApplicationError
{
    public string Code { get; init; }
    public string Description { get; init; }
    
    public ApplicationError(string code, string description)
    {
        Code = code;
        Description = description;
    }
    
    public static ApplicationError Unexpected()
    {
        return new ApplicationError("UnexpectedError", "An unexpected error occurred.");
    }
}