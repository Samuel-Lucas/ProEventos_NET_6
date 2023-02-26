using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;
using ProEventos.Domain.Models;

namespace ProEventos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventosController : ControllerBase
{
    private readonly IEventosServices _eventosServices;

    public EventosController(IEventosServices eventosServices)
    {
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
                // DeleteImage(evento.ImageUrl)
                // evento.ImagemUrl = SaveImage(file);
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
            if (await _eventosServices.DeleteEvento(id))
                return Ok(new { message = "Deletado"});
            else
                return BadRequest("Evento não deletado");
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar deletar evento. Erro {ex.Message}");
        }
    }
}