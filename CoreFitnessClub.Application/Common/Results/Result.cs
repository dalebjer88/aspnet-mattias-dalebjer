namespace CoreFitnessClub.Application.Common.Results;

public class Result
{
    public bool Succeeded { get; }
    public string? ErrorMessage { get; }

    protected Result(bool succeeded, string? errorMessage)
    {
        Succeeded = succeeded;
        ErrorMessage = errorMessage;
    }

    public static Result Success()
    {
        return new Result(true, null);
    }

    public static Result Failure(string errorMessage)
    {
        return new Result(false, errorMessage);
    }
}