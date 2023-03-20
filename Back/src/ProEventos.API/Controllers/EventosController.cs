using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;

namespace ProEventos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventosController : ControllerBase
{
    private readonly IEventosServices _eventosServices;
    private readonly IWebHostEnvironment _hostEnvironment;

    public EventosController(IEventosServices eventosServices, IWebHostEnvironment hostEnvironment)
    {
        _hostEnvironment = hostEnvironment;
        _eventosServices = eventosServices;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            var eventos = await _eventosServices.GetAllEventosAsync(true);
            if (eventos == null)
                return NoContent();
            
            return Ok(eventos);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar eventos. Erro {ex.Message} \n {ex.StackTrace}");
        }
    }

    [HttpGet("BuscaEventoPorId/{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var evento = await _eventosServices.GetEventoByIdAsync(id, true);
            if (evento == null)
                return NoContent();
            
            return Ok(evento);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar evento. Erro {ex.Message}");
        }
    }

    [HttpGet("BuscaEventoPorTema/{tema}")]
    public async Task<IActionResult> GetByTema(string tema)
    {
        try
        {
            var evento = await _eventosServices.GetAllEventosByTemaAsync(tema, true);
            if (evento == null)
                return NoContent();
            
            return Ok(evento);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar evento. Erro {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Post(EventoDto model)
    {
        try
        {
            var evento = await _eventosServices.AddEvento(model);
            if (evento == null)
                return BadRequest($"Erro ao tentar incluir evento");
            
            return Ok(evento);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar evento. Erro {ex.Message}");
        }
    }

    [HttpPost("upload-image/{eventoId}")]
    public async Task<IActionResult> UploadImage(int eventoId)
    {
        try
        {
            var evento = await _eventosServices.GetEventoByIdAsync(eventoId, true);
            if (evento == null) return NoContent();

            var file = Request.Form.Files[0];
            if (file.Length > 0) 
            {
                DeleteImage(evento.ImagemUrl);
                evento.ImagemUrl = await SaveImage(file);
            }

            var eventoRetorno = await _eventosServices.UpdateEvento(eventoId, evento);
            
            return Ok(evento);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar evento. Erro {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, EventoDto model)
    {
        try
        {
            var evento = await _eventosServices.UpdateEvento(id, model);
            if (evento == null)
                return BadRequest($"Erro ao tentar atualizar evento");
            
            return NoContent();
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar evento. Erro {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var evento = await _eventosServices.GetEventoByIdAsync(id, true);
            if (evento == null) return NoContent();

            if (await _eventosServices.DeleteEvento(id))
            {
                DeleteImage(evento.ImagemUrl);
                return Ok(new { message = "Deletado"});
            }
            else
            {
                throw new Exception("Ocorreu um erro inesperado ao tentar deletar o evento");
            }
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar deletar evento. Erro {ex.Message}");
        }
    }

    [NonAction]
    public async Task<string> SaveImage(IFormFile imageFile)
    {
        string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.Name)
                                            .Take(10)
                                            .ToArray()
                                    ).Replace(' ', '-');

        imageName = $"{imageName}{DateTime.UtcNow.ToString("yyyymmssfff")}{Path.GetExtension(imageFile.FileName)}";

        var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, @"Resources/Images", imageName);

        using (var fileStream = new FileStream(imagePath, FileMode.Create))
        {
            await imageFile.CopyToAsync(fileStream);
        }

        return imageName;
    }

    [NonAction]
    public void DeleteImage(string imageName)
    {
        var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, @"Resources/Images", imageName);
        if (System.IO.File.Exists(imagePath))
            System.IO.File.Delete(imagePath);
    }
}