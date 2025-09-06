using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


/// <summary>
/// Gestiona propiedades inmobiliarias.
/// </summary>
[ApiController]
[Route("api/v1/properties")]
public class PropertiesController : ControllerBase
{
    private readonly IMediator _mediator;
    public PropertiesController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    /// Obtiene el listado de propiedades con sus imágenes.
    /// </summary>
    /// <param name="page">Número de página.</param>
    /// <param name="pageSize">Cantidad por página.</param>
    /// <returns>Lista de propiedades.</returns>
    [HttpGet("property-list")]
    [Authorize(Policy = "Api.Read")]
    public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var list = await _mediator.Send(new GetPropertiesQuery(page, pageSize));
        return Ok(list);
    }
    /// <summary>
    /// Inserta nuevas propiedades 
    /// </summary>
    /// <param name="Name">Nombre de la propiedad.</param>
    /// <param name="Address">Dirección de la propiedad.</param>
    /// <param name="Price">Precio de la propiedad.</param>
    /// <param name="CodeExternal">Código de la propiedad.</param>
    /// <param name="OwnerIdOwner">Id del propietario.</param>
    /// <param name="Enabled">Si está o no activa la propiedad.</param>
    /// <returns>Id de la propiedad agregada.</returns>
    [HttpPost("insert-propery")]
    [Authorize(Policy = "Api.Write")]
    public async Task<IActionResult> Create([FromBody] CreatePropertyCommand cmd)
    {
        var id = await _mediator.Send(cmd);
        return CreatedAtAction(nameof(Get), new { id }, new { id });
    }

    /// <summary>
    /// Inserta imagenes de  una propiedad
    /// </summary>
    /// <param name="IdProperty">Id de la propiedad a la cual se le cargará la imagen.</param>
    /// <param name="FileName">Nombre de la imagen que se cargará.</param>
    /// <param name="Base64">Imagen convertida a Base64.</param>
    /// <param name="IsDefault">Imagen por defecto.</param>
    /// <param name="IsEnabled">Si la imagen está activa o no.</param>
    /// <returns>Mensaje: "Imagen cargada correctamente."</returns>
    [HttpPost("upload-image")]
    [Authorize(Policy = "Api.Write")]
    public async Task<IActionResult> UploadImage(Guid id, [FromBody] UploadPropertyImageCommand cmd)
    {
        if (id != cmd.PropertyId)
            return BadRequest("El Id de la propiedad no coincide.");
        var result = await _mediator.Send(cmd);
        if (!result)
            return NotFound("No se encontró la propiedad.");

        return Ok("Imagen cargada correctamente.");
    }

    /// <summary>
    /// Actualiza propiedades 
    /// </summary>
    /// <param name="Name">Nombre de la propiedad.</param>
    /// <param name="Address">Dirección de la propiedad.</param>
    /// <param name="Price">Precio de la propiedad.</param>
    /// <param name="CodeExternal">Código de la propiedad.</param>
    /// <param name="OwnerIdOwner">Id del propietario.</param>
    /// <param name="Enabled">Si está o no activa la propiedad.</param>
    /// <returns>Id de la propiedad actualizada.</returns>
    [HttpPut("update-property")]
    [Authorize(Policy = "Api.Write")]
    public async Task<IActionResult> UpdateProperty(Guid id, [FromBody] UpdatePropertyCommand cmd)
    {
        if (id != cmd.IdProperty)
            return BadRequest("El id de la propiedad no coincide.");
        var result = await _mediator.Send(cmd);
        return CreatedAtAction(nameof(Get), new { id }, new { id });
    }

    /// <summary>
    /// Modifica el precio de la propiedad y agrega un historico de la propiedad 
    /// </summary>
    /// <param name="IdProperty">Id de la propiedad que se actualizará su precio.</param>
    /// <param name="NewPrice">Nuevo precio de la propiedad.</param>
    /// <param name="Note">Comentarios de la actualización de precio.</param>  
    /// <returns>Id de la propiedad actualizada.</returns>
    [HttpPatch("cahnge-price-property")]
    [Authorize(Policy = "Api.Write")]
    public async Task<IActionResult> UpdatePriceProperty(Guid id, [FromBody] ChangePropertyPriceCommand cmd)
    {
        if (id != cmd.IdProperty)
            return BadRequest("El id de la propiedad no coincide.");
        var result = await _mediator.Send(cmd);
        return CreatedAtAction(nameof(Get), new { id }, new { id });
    }
}