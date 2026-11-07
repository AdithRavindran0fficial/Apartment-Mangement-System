namespace Nanma.Domain.Identity;

public class Profile
{
    public Guid UserId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? AlternatePhoneNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? AddressLine { get; set; }

    public User User { get; set; } = null!;
}
