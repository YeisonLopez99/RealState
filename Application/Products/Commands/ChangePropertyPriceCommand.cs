using MediatR;

public record ChangePropertyPriceCommand(Guid IdProperty, decimal NewPrice, string? Note) : IRequest<Guid>;