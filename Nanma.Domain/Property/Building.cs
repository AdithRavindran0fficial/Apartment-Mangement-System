namespace Nanma.Domain.Property;

public class Building
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string? AddressLine { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public ICollection<Apartment> Apartments { get; set; } = new List<Apartment>();
}
