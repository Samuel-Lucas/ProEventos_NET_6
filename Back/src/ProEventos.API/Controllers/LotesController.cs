using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;
using ProEventos.Domain.Models;

namespace ProEventos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LotesController : ControllerBase
{
    private readonly ILoteServices _loteServices;

    public LotesController(ILoteServices loteServices)
    {
        _loteServices = loteServices;
    }

    [HttpGet("{eventoId}")]
    public async Task<IActionResult> Get(int eventoId)
    {
        try
        {
            var eventos = await _loteServices.GetEventoByIdAsync(true);
            if (eventos == null)
                return NoContent();
            
            return Ok(eventos);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar eventos. Erro {ex.Message} \n {ex.StackTrace}");
        }
    }

    [HttpPut("{eventoId}")]
    public async Task<IActionResult> Put(int eventoId, EventoDto model)
    {
        try
        {
            var evento = await _loteServices.UpdateEvento(eventoId, model);
            if (evento == null)
                return BadRequest($"Erro ao tentar atualizar lote");
            
            return NoContent();
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar lote. Erro {ex.Message}");
        }
    }

    [HttpDelete("{eventoId}/{loteId}")]
    public async Task<IActionResult> Delete(int eventoId, int loteId)
    {
        try
        {
            if (await _loteServices.DeleteEvento(id))
                return Ok(new { message = "Deletado"});
            else
                return BadRequest("Lote não deletado");
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar deletar lote. Erro {ex.Message}");
        }
    }
}