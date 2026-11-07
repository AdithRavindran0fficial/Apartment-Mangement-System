namespace Nanma.Domain.Property;

public class ApartmentUser
{
    public Guid ApartmentId { get; set; }
    public Guid UserId { get; set; }
    public bool IsPrimaryResident { get; set; }
    public DateTime JoinedAtUtc { get; set; } = DateTime.UtcNow;

    public Apartment Apartment { get; set; } = null!;
    public Identity.User User { get; set; } = null!;
}
