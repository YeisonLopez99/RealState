namespace Domain.Entities;

public class PropertyTrace
{
    public Guid IdPropertyTrace { get; private set; } = Guid.NewGuid();
    public Guid PropertyIdProperty { get; private set; }
    public Property Property { get; private set; } = default!;
    public DateTime DateSale { get; private set; }
    public string Name { get; private set; } = default!;
    public decimal Value { get; private set; }
    public decimal? Tax { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    private PropertyTrace() { }
    public PropertyTrace(Guid propertyId, DateTime dateSale, string name, decimal value, string? note)
    {
        PropertyIdProperty = propertyId; DateSale = dateSale; Name = name; Value = value; Tax = null;
    }
}