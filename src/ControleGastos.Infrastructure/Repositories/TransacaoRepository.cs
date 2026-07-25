using ControleGastos.Application.Interfaces;
using ControleGastos.Domain;
using Microsoft.EntityFrameworkCore;

namespace ControleGastos.Infrastructure.Repositories;

/// <summary>
/// Implementação concreta da interface <see cref="ITransacaoRepository"/>, responsável por traduzir as operações de acesso a dados de Transação em consultas e comandos reais contra o banco.
/// </summary>
/// <param name="context">Sessão com o banco de dados, injetada pelo container de DI do ASP.NET Core.</param>
public class TransacaoRepository(ControleGastosDbContext context) : ITransacaoRepository
{
    /// <summary>
    /// Consulta todas as transações cadastradas, incluindo os dados da Pessoa vinculada a cada uma.
    /// Útil caso alguma camada superior precise exibir o nome da pessoa junto com a transação.
    /// </summary>
    /// <returns>Lista de todas as transações.</returns>
    public Task<List<Transacao>> ListarTodasAsync() =>
        context.Transacoes.Include(t => t.Pessoa).ToListAsync();

    /// <summary>
    /// Adiciona uma nova transação ao contexto de rastreamento do EF Core.
    /// A gravação efetiva no banco só ocorre ao chamar <see cref="SalvarAsync"/>.
    /// </summary>
    /// <param name="transacao">Transação a ser adicionada.</param>
    public async Task AdicionarAsync(Transacao transacao) => await context.Transacoes.AddAsync(transacao);

    /// <summary>
    /// Persiste no banco todas as alterações pendentes rastreadas pelo DbContext.
    /// </summary>
    public Task SalvarAsync() => context.SaveChangesAsync();
}