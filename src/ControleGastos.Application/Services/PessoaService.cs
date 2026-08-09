using ControleGastos.Application.Interfaces;
using ControleGastos.Domain;

namespace ControleGastos.Application.Services;

/// <summary>
/// Serviço responsável por definidir todas as regras de negócio que serão executadas na camada dos métodos correspondentes.
/// </summary>
/// <param name="repository">Contrato de acesso a dados de Pessoa, implementado na camada de Infrastructure.</param>
public class PessoaService(IPessoaRepository repository)
{
    /// <summary>
    /// Lista todas as pessoas cadastradas no sistema.
    /// </summary>
    /// <returns>Lista de todas as pessoas, sem filtro.</returns>
    public Task<List<Pessoa>> ListarTodasAsync() => repository.ListarTodasAsync();

    /// <summary>
    /// Cria uma pessoa no banco, validando os dados obrigatórios antes de persistir o registro.
    /// </summary>
    /// <param name="nome">Nome da pessoa, não pode estar vazio.</param>
    /// <param name="idade">Idade da pessoa, não pode estar vazio.</param>
    /// <returns>Objeto da pessoa recém-criada, já com o Id gerado pelo banco.</returns>
    /// <exception cref="ArgumentException">Lançada quando o nome está vazio/em branco, ou quando a idade é negativa.</exception>
    public async Task<Pessoa> CriarAsync(string nome, int idade, string? email)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome é obrigatório.");

        if (idade < 0)
            throw new ArgumentException("Idade não pode ser negativa.");

        var pessoa = new Pessoa { Nome = nome, Idade = idade, Email = email };

        await repository.AdicionarAsync(pessoa);
        await repository.SalvarAsync();

        return pessoa;
    }
    
    /// <summary>
    /// Remove uma pessoa da base de dados. Todas as transações vinculadas a essa pessoa são removidas automaticamente junto.
    /// </summary>
    /// <param name="id">Identificador unico da pessoa que será removida</param>
    /// <returns>
    /// true se a pessoa existia e foi removida com sucesso;
    /// false se não foi encontrada nenhuma pessoa com o id informado.
    /// </returns>
    public async Task<bool> DeletarAsync(int id)
    {
        var pessoa = await repository.ObterPorIdAsync(id);
        if (pessoa is null) return false;

        await repository.RemoverAsync(pessoa);
        await repository.SalvarAsync();
        return true;
    }
    
}