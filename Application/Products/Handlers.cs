using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class CreatePropertyCommandHandler : IRequestHandler<CreatePropertyCommand, Guid>
{
    private readonly IRepository<Property> _repo;
    public CreatePropertyCommandHandler(IRepository<Property> repo) => _repo = repo;
    public async Task<Guid> Handle(CreatePropertyCommand request, CancellationToken cancellationToken)
    {
        var entity = new Property(request.Name, request.Address, request.Price, request.OwnerIdOwner, request.CodeExternal);
        await _repo.AddAsync(entity, cancellationToken);
        await _repo.SaveChangesAsync(cancellationToken);
        return entity.IdProperty;
    }
}

public class GetPropertiesQueryHandler : IRequestHandler<GetPropertiesQuery, List<PropertyDto>>
{
    private readonly IRepository<Property> _repo;
    private readonly IRepository<PropertyImage> _repoImage;
    private readonly IMapper _mapper;
    public GetPropertiesQueryHandler(IRepository<PropertyImage> repoImage, IRepository<Property> repo, IMapper mapper) { _repoImage = repoImage; _repo = repo; _mapper = mapper; }
    public async Task<List<PropertyDto>> Handle(GetPropertiesQuery request, CancellationToken cancellationToken)
    {
        var list = await _repo.ListAsync(q => q.Include(p => p.Images), cancellationToken);
        var page = list.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).ToList();
        return _mapper.Map<List<PropertyDto>>(page);
    }
}
public class UploadPropertyImageHandler : IRequestHandler<UploadPropertyImageCommand, bool>
{   
    private readonly IRepository<Property> _repoProperty;
    private readonly IRepository<PropertyImage> _repoImage;

    public UploadPropertyImageHandler(IRepository<PropertyImage> repositoryImage, IRepository<Property> repositoryProperty) { _repoProperty = repositoryProperty; _repoImage = repositoryImage; }

    public async Task<bool> Handle(UploadPropertyImageCommand request, CancellationToken cancellationToken)
    {
        var property = await _repoProperty.GetByIdAsync(request.PropertyId);       
        if (property == null)
            return false;
        var propertyImage = property.AddImage(request.FileName, request.ImageBase64, request.IsDefault);       
         
         await _repoImage.AddAsync(propertyImage);
     
        await _repoImage.SaveChangesAsync(cancellationToken);

        return true;
    }
}

public class UpdatePropertyCommandHandler : IRequestHandler<UpdatePropertyCommand, Guid>
{
    private readonly IRepository<Property> _repo;

    public UpdatePropertyCommandHandler(IRepository<Property> repo)
    {
        _repo = repo;
    }

    public async Task<Guid> Handle(UpdatePropertyCommand request, CancellationToken cancellationToken)
    {       
        var property = await _repo.GetByIdAsync(request.IdProperty, cancellationToken);
        if (property == null)
            throw new KeyNotFoundException($"Propiedad con Id {request.IdProperty} no encontrada");
        
        property.Update(request.Name, request.Address, request.Price, request.CodeExternal);
      
        _repo.Update(property);
        await _repo.SaveChangesAsync(cancellationToken);

        return property.IdProperty;
    }
}

public class ChangePropertyPriceHandler : IRequestHandler<ChangePropertyPriceCommand, Guid>
{
    private readonly IRepository<Property> _repo;
    private readonly IRepository<PropertyTrace> _repoTrace;
    private static object LockProcessingTrace = new object();
    private static object LockProcessingProperty = new object();

    public ChangePropertyPriceHandler(IRepository<Property> repo, IRepository<PropertyTrace> repoTrace)
    {
        _repo = repo;
        _repoTrace = repoTrace;
    }

    public async Task<Guid> Handle(ChangePropertyPriceCommand request, CancellationToken cancellationToken)
    {

        var property = await _repo.GetByIdAsync(request.IdProperty, cancellationToken);
        if (property == null)
            throw new KeyNotFoundException($"Property with Id {request.IdProperty} not found");

        var trace = new PropertyTrace(property.IdProperty, DateTime.UtcNow, property.Name, request.NewPrice, request.Note);
        await _repoTrace.AddAsync(trace);
        await _repoTrace.SaveChangesAsync(cancellationToken);

        property.Update(property.Name, property.Address, request.NewPrice, property.CodeExternal);
        _repo.Update(property);
        await _repo.SaveChangesAsync();
       
        return property.IdProperty;
    }
}