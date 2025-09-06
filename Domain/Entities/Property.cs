namespace Domain.Entities;

public class    Property
{
    public Guid IdProperty { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; } = default!;
    public string Address { get; private set; } = default!;
    public decimal Price { get; private set; }
    public string? CodeExternal { get; private set; }   
    public Guid OwnerIdOwner { get; private set; }
    public Owner Owner { get; private set; } = default!;
    public bool Enabled { get; private set; } = true;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; private set; }

    private readonly List<PropertyImage> _images = new();
    public IReadOnlyCollection<PropertyImage> Images => _images.AsReadOnly();

    private readonly List<PropertyTrace> _traces = new();
    public IReadOnlyCollection<PropertyTrace> Traces => _traces.AsReadOnly();

    private Property() { }

    public Property(string name, string address, decimal price, Guid ownerIdOwner, string? codeExternal)
    {
        Name = name; Address = address; Price = price;  OwnerIdOwner= ownerIdOwner; CodeExternal = codeExternal;
    }

    public void Update(string name, string address, decimal price, string? codeExternal)
    {
        Name = name; Address = address; Price = price; CodeExternal = codeExternal; UpdatedAt = DateTime.UtcNow;
    }

    public PropertyImage AddImage(string fileName, string base64, bool isDefault)
    {
        var img = new PropertyImage(this.IdProperty, fileName, base64, isDefault);
        _images.Add(img);
        return img;
    }

    public void ChangePrice(decimal newPrice, string? note)
    {
        if (newPrice <= 0) throw new ArgumentException("Price must be > 0");
        var trace = new PropertyTrace(this.IdProperty, DateTime.UtcNow, "PriceChange", newPrice, note);
        _traces.Add(trace);
        Price = newPrice;
        UpdatedAt = DateTime.UtcNow;
    }
}