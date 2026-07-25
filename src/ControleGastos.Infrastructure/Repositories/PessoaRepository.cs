using ControleGastos.Application.Interfaces;
using ControleGastos.Domain;
using Microsoft.EntityFrameworkCore;

namespace ControleGastos.Infrastructure.Repositories;

/// <summary>
/// Implementação concreta da interface <see cref="IPessoaRepository"/>.
/// Responsável por transmitir as operações realizadas em comandos para o banco de dados, utilizando o EntityFramework.
/// </summary>
/// <param name="context">Sessão com o banco de dados, injetada pelo container de DI do ASP.NET Core.</param>
public class PessoaRepository(ControleGastosDbContext context) : IPessoaRepository
{
    
    /// <summary>
    /// Consulta todas as pessoas cadastradas no banco.
    /// </summary>
    /// <returns>Lista de todas as pessoas.</returns>
    public Task<List<Pessoa>> ListarTodasAsync() => context.Pessoas.ToListAsync();
    
    /// <summary>
    /// Consulta uma pessoa específica através do Identificador Unico (ID), daquela pessoa.
    /// </summary>
    /// <param name="id">Identificador Único de cada pessoa.</param>
    /// <returns>A pessoa encontrada, ou null caso não exista nenhuma com esse ID.</returns>
    public Task<Pessoa?> ObterPorIdAsync(int id) => context.Pessoas.FindAsync(id).AsTask();

    /// <summary>
    /// Adiciona uma nova pessoa no contexto EF Core.
    /// A pessoa somente é registrada no banco quando o <see cref="SalvarAsync"/> é chamado.
    /// </summary>
    /// <param name="pessoa">Pessoa que será adicionada.</param>
    public async Task AdicionarAsync(Pessoa pessoa) => await context.Pessoas.AddAsync(pessoa);

    /// <summary>
    /// Seleciona a pessoa inserida para a remoção.
    /// A pessoa somente é removida do banco quando o <see cref="SalvarAsync"/> é chamado.
    /// </summary>
    /// <param name="pessoa">Pessoa que será removida.</param>
    public Task RemoverAsync(Pessoa pessoa)
    {
        context.Pessoas.Remove(pessoa);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Persiste no banco as alterações que estão pendentes de execução e estão registradas no contexto do banco de dados.
    /// </summary>
    public Task SalvarAsync() => context.SaveChangesAsync();
}