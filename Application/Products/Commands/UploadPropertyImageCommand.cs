
using MediatR;


    public record UploadPropertyImageCommand (Guid PropertyId, string FileName, string ImageBase64, bool IsDefault) : IRequest<bool>;
       

