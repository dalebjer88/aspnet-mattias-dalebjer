namespace CoreFitnessClub.Domain.Entities;

public abstract class BaseEntity
{
    public int Id { get; protected set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime UpdatedAtUtc { get; private set; }
}