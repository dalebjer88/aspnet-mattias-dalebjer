namespace CoreFitnessClub.Application.Features.Memberships;

public class MembershipResult
{
    public bool Succeeded { get; set; }
    public string Error { get; set; } = string.Empty;

    public static MembershipResult Success()
    {
        return new MembershipResult { Succeeded = true };
    }

    public static MembershipResult Failure(string error)
    {
        return new MembershipResult
        {
            Succeeded = false,
            Error = error
        };
    }
}