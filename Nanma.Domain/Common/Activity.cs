namespace Nanma.Domain.Common;

public class Activity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid? ApartmentId { get; set; }
    public string ActivityLog { get; set; } = string.Empty;
    public string? PageValue { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public Identity.User User { get; set; } = null!;
}
