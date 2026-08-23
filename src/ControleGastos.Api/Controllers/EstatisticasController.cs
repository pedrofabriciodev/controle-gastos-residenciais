using ControleGastos.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ControleGastos.Api.Controllers;

/// <summary>
/// Controller responsável por expor as estatísticas processadas
/// a partir dos dados cadastrados no sistema.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class EstatisticasController(EstatisticaService service) : ControllerBase
{
    /// <summary>
    /// Consulta as estatísticas gerais: maior/menor valor, média,
    /// quantidade total, contagem por tipo e ranking de pessoas por saldo.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Consultar()
    {
        return Ok(await service.ConsultarEstatisticasAsync());
    }
}