using MediatR;

public record UpdatePropertyCommand(Guid IdProperty, string Name, string Address, decimal Price, string? CodeExternal) : IRequest<Guid>;