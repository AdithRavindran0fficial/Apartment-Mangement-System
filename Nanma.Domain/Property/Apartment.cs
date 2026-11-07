namespace Nanma.Domain.Property;

public class Apartment
{
    public Guid Id { get; set; }
    public Guid BuildingId { get; set; }
    public string ApartmentNumber { get; set; } = string.Empty;
    public int Floor { get; set; }
    public bool IsOccupied { get; set; }
    public bool IsActive { get; set; } = true;

    public Building Building { get; set; } = null!;
    public ICollection<ApartmentUser> ApartmentUsers { get; set; } = new List<ApartmentUser>();
}
