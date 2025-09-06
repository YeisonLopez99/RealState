using MediatR;

public record CreatePropertyCommand(string Name, string Address, decimal Price, Guid OwnerIdOwner, string? CodeExternal) : IRequest<Guid>;