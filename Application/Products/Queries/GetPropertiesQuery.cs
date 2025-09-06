using MediatR;

public record GetPropertiesQuery(int Page = 1, int PageSize = 10, Guid? OwnerId = null) : IRequest<List<PropertyDto>>;