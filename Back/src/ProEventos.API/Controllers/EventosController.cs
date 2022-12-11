using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Interfaces;
using ProEventos.Domain.Models;
using ProEventos.Persistence.Context;

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
                return NotFound("Nenhum evento encontrado");
            
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
            var evento = await _eventosServices.GetAllEventosByIdAsync(id, true);
            if (evento == null)
                return NotFound($"Nenhum evento encontrado do id: {id}");
            
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
                return NotFound($"Nenhum evento encontrado com tema: {tema}");
            
            return Ok(evento);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar evento. Erro {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Post(Evento model)
    {
        try
        {
            var evento = await _eventosServices.AddEventos(model);
            if (evento == null)
                return BadRequest($"Erro ao tentar incluir evento");
            
            return NoContent();
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar evento. Erro {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Evento model)
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
                return Ok($"Deletado");
            else
                return BadRequest("Evento não deletado");
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar deletar evento. Erro {ex.Message}");
        }
    }
}