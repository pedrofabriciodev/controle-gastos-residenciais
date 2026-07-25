using ControleGastos.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ControleGastos.Api.Controllers;

/// <summary>
/// Controller responsável por gerenciar as pessoas no sistema.
/// </summary>
/// <param name="service">Serviço referente a entidade Pessoa.</param>
[ApiController]
[Route("api/[controller]")]
public class PessoasController(PessoaService service) : ControllerBase
{
    
    /// <summary>
    /// Função GET responsável por listar todas as pessoas da base de dados.
    /// </summary>
    /// <returns>Retorna a lista com todas as pessoas cadastradas.</returns>
    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        return Ok(await service.ListarTodasAsync());
    }

    /// <summary>
    /// Função POST resppnsável por criar o registro das pessoas na base de dados.
    /// </summary>
    /// <param name="request">Dados filtrados que serão retornados após a criação do objeto Pessoa.</param>
    /// <returns>Retorna a criaçaõ do registro pessoa no banco de dados.</returns>
    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarPessoaRequest request)
    {
        try
        {
            var pessoa = await service.CriarAsync(request.Nome, request.Idade);
            return CreatedAtAction(nameof(Listar), new { id = pessoa.Id }, pessoa);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Função DELETE responsável pela exclusão do registro de uma pessoa da base de dados.
    /// Ao remover a pessoa, todas as transações vinculadas a ela também são excluídas automaticamente.
    /// </summary>
    /// <param name="id">Identificador único da pessoa que será removida da base.</param>
    /// <returns>
    /// No Content caso a pessoa seja encontrada e removida com sucesso;
    /// Not Found caso não exista pessoa cadastrada com o id informado.
    /// </returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Deletar(int id)
    {
        var sucesso = await service.DeletarAsync(id);
        return sucesso ? NoContent() : NotFound();
    }
}

/// <summary>
/// Dados filtrados, necessários para o registro de uma pessoa.
/// Expões somente os campos necessários e "publicos" no momento da criação.
/// </summary>
/// <param name="Nome">Nome da pessoa cadastrada.</param>
/// <param name="Idade">Idade da pessoa cadastrada</param>
public record CriarPessoaRequest(string Nome, int Idade);