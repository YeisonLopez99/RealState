using Application.Products.DTOs;

public class PropertyDto
{
    public Guid IdProperty { get; set; }
    public string Name { get; set; } = default!;
    public string Address { get; set; } = default!;
    public decimal Price { get; set; }
    public string? CodeExternal { get; set; }
    public List<PropertyImageDto>? Images { get; set; }
}