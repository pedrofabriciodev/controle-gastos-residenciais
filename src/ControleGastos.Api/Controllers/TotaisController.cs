using ControleGastos.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ControleGastos.Api.Controllers;

/// <summary>
/// Controller responsável por gerenciar os totais no sistema.
/// </summary>
/// <param name="service">Serviço responsável por calcular os totais a partir das pessoas e transações cadastradas.</param>
[ApiController]
[Route("api/[controller]")]
public class TotaisController(TotalService service) : ControllerBase
{
    /// <summary>
    /// Consulta os totais de receitas, despesas e saldo de cada pessoa cadastrada, além do total geral somando todas elas.
    /// </summary>
    /// <returns>OK com o resumo financeiro por pessoa e o total geral.</returns>
    [HttpGet]
    public async Task<IActionResult> Consultar()
    {
        return Ok(await service.ConsultarTotaisAsync());
    }
}