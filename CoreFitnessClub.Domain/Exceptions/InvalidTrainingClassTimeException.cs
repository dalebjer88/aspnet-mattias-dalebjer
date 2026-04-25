namespace CoreFitnessClub.Domain.Exceptions;

public class InvalidTrainingClassTimeException : DomainException
{
    public InvalidTrainingClassTimeException()
        : base("End time must be later than start time.")
    {
    }
}