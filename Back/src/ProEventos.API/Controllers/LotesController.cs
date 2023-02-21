using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;

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
            var lotes = await _loteServices.GetLotesByEventoIdAsync(eventoId);
            if (lotes == null)
                return NoContent();
            
            return Ok(lotes);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar lotes. Erro {ex.Message} \n {ex.StackTrace}");
        }
    }

    [HttpPut("{eventoId}")]
    public async Task<IActionResult> Put(int eventoId, LoteDto[] model)
    {
        try
        {
            var lotes = await _loteServices.SaveLoteAsync(eventoId, model);
            if (lotes == null)
                return NoContent();
            
            return Ok(lotes);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar salvar lote. Erro {ex.Message}");
        }
    }

    [HttpDelete("{eventoId}/{loteId}")]
    public async Task<IActionResult> Delete(int eventoId, int loteId)
    {
        try
        {
            var lote = await _loteServices.GetLoteByIdsAsync(eventoId, loteId);
            if (lote == null) return NoContent();

            return await _loteServices.DeleteLoteAsync(lote.EventoId, lote.Id)
                        ? Ok(new { message = "Lote Deletado"} )
                        : throw new Exception("Ocorreu um problema ao deletar o lote");
               
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar deletar lote. Erro {ex.Message}");
        }
    }
}