using ControleGastos.Domain;

namespace ControleGastos.Application.Interfaces;

/// <summary>
/// Contrato de acesso a dados de Pessoa. Define as operações de persistência necessárias.
/// </summary>
public interface IPessoaRepository
{
    /// <summary>
    /// Lista todas as pessoas cadastradas.
    /// </summary>
    /// <returns>Lista de todas as pessoas cadastradas.</returns>
    Task<List<Pessoa>> ListarTodasAsync();
    
    /// <summary>
    /// Busca uma pessoa pelo seu identificador único.
    /// </summary>
    /// <param name="id">Identificador único da pessoa.</param>
    /// <returns>A pessoa encontrada, ou null caso não exista.</returns>
    Task<Pessoa?> ObterPorIdAsync(int id);
    
    /// <summary>
    /// Adiciona uma nova pessoa ao rastreamento do contexto de dados.
    /// </summary>
    /// <param name="pessoa"></param>
    Task AdicionarAsync(Pessoa pessoa);
    
    /// <summary>
    /// Marca uma pessoa para remoção.
    /// </summary>
    /// <param name="pessoa">Pessoa a ser removida.</param>
    Task RemoverAsync(Pessoa pessoa);
    
    /// <summary>
    /// Persiste no banco todas as alterações pendentes.
    /// </summary>
    Task SalvarAsync();
    
}