namespace Domain.Entities;

public class PropertyImage
{
    public Guid IdPropertyImage { get; private set; } = Guid.NewGuid();
    public Guid IdProperty { get; private set; }
    public Property Property { get; private set; } = default!;
    public string FileName { get; private set; } = default!;
    public string Base64 { get; private set; } = default!;
    public bool IsDefault { get; private set; }
    public bool Enabled { get; private set; } = true;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    private PropertyImage() { }

    public PropertyImage(Guid propertyId, string fileName, string base64, bool isDefault)
    {
        IdProperty = propertyId; FileName = fileName; Base64 = base64; IsDefault = isDefault;
    }

    public void Disable() => Enabled = false;
}