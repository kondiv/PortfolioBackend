namespace Services.Results;

public class Result
{
    private static readonly Result _success = new Result() { Succeeded = true };
    
    private readonly List<Error> _errors = new List<Error>();
    
    public bool Succeeded { get; protected set; }

    public IReadOnlyCollection<Error> Errors => _errors.AsReadOnly();
    
    public static Result Success => _success;

    public static Result Failed(params Error[]? errors)
    {
        var result = new Result() { Succeeded = false };

        if (errors != null)
        {
            result._errors.AddRange(errors);
        }

        return result;
    }

    public static Result Failed(List<Error>? errors)
    {
        var result = new Result() { Succeeded = false };

        if (errors != null)
        {
            result._errors.AddRange(errors);
        }

        return result;
    }
}