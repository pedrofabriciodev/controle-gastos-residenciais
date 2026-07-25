using ControleGastos.Domain;

namespace ControleGastos.Application.Interfaces;

/// <summary>
/// Contrato de acesso a dados de Transação. Define as operações de persistência necessárias.
/// </summary>
public interface ITransacaoRepository
{
    /// <summary>
    /// Lista todas as transações realizadas.
    /// </summary>
    /// <returns>Lista de todas as transações realizadas.</returns>
    Task<List<Transacao>> ListarTodasAsync();
    
    /// <summary>
    /// Adiciona uma nova transação ao rastreamento do contexto de dados.
    /// </summary>
    /// <param name="transacao">Transação a ser adicionada.</param>
    Task AdicionarAsync(Transacao transacao);
    
    /// <summary>
    /// Persiste no banco todas as alterações pendentes.
    /// </summary>
    Task SalvarAsync();
}