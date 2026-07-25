using ControleGastos.Application.Services;
using ControleGastos.Domain;
using Microsoft.AspNetCore.Mvc;

namespace ControleGastos.Api.Controllers;

/// <summary>
/// Controller responsável por gerenciar as transações no sistema.
/// </summary>
/// <param name="service">Serviço referente a entidade Transação</param>
[ApiController]
[Route("api/[controller]")]
public class TransacoesController(TransacaoService service) : ControllerBase
{
    
    /// <summary>
    /// Função GET responsável por listar todas as transações da base de dados.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        return Ok(await service.ListarTodasAsync());
    }

    /// <summary>
    /// Função POST responsável por o registro das transações na base de dados.
    /// </summary>
    /// <param name="request">Dados filtrados que serão retornados após a criação do objeto Transacao.</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarTransacaoRequest request)
    {
        try
        {
            var transacao = await service.CriarAsync(
                request.Descricao, request.Valor, request.Tipo, request.PessoaId);

            return CreatedAtAction(nameof(Listar), new { id = transacao.Id }, transacao);
        }
        catch (InvalidOperationException ex)
        {
            return UnprocessableEntity(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

/// <summary>
/// Dados filtrados, necessários para o registro de uma transaçõa.
/// Expões somente os campos necessários e "publicos" no momento da criação.
/// </summary>
/// <param name="Descricao">Descrição da transação.</param>
/// <param name="Valor">Valor da transação.</param>
/// <param name="Tipo">Tipo da transação (receita ou despesa).</param>
/// <param name="PessoaId">Identificador Único (ID) da pessoa que esta realizando a transação. </param>
public record CriarTransacaoRequest(string Descricao, decimal Valor, TipoTransacao Tipo, int PessoaId);