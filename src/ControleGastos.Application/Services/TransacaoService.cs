using ControleGastos.Application.DTOs;
using ControleGastos.Application.Interfaces;
using ControleGastos.Domain;

namespace ControleGastos.Application.Services;

/// <summary>
/// Serviço responsável por definir todas as regras de negócio relacionadas à entidade Transação. 
/// </summary>
/// <param name="transacaoRepository">Contrato de acesso a dados de Transação.</param>
/// <param name="pessoaRepository">Contrato de acesso a dados de Pessoa, necessário para validar a idade da pessoa informada.</param>
public class TransacaoService(ITransacaoRepository transacaoRepository, IPessoaRepository pessoaRepository)
{
    /// <summary>
    /// Lista todas as transações cadastradas no sistema.
    /// </summary>
    /// <returns>Lista de todas as transações, já convertidas para o formato de retorno da API.</returns>
    public async Task<List<TransacaoDto>> ListarTodasAsync()
    {
        var transacoes = await transacaoRepository.ListarTodasAsync();
        return transacoes.Select(TransacaoDto.FromEntity).ToList();
    }

    /// <summary>
    /// Cria uma transação, validando os dados obrigatórios, a existência da pessoa informada, e a regra de negócio de menor de idade antes de persistir.
    /// </summary>
    /// <param name="descricao">Descrição da transação. Não pode ser vazia.</param>
    /// <param name="valor">Valor da transação. Deve ser maior que zero.</param>
    /// <param name="tipo">Tipo da transação: Receita ou Despesa.</param>
    /// <param name="pessoaId">Identificador único da pessoa dona da transação. Precisa existir no cadastro de pessoas.</param>
    /// <returns>A transação recém-criada, já convertida para o formato de retorno da API.</returns>
    /// <exception cref="ArgumentException">Lançada quando a descrição está vazia, o valor não é maior que zero, ou quando a pessoa informada não existe.</exception>
    /// <exception cref="InvalidOperationException">Lançada quando a pessoa é menor de idade e o tipo informado é Receita.
    /// Pela regra de negócio, menor de idade somente pode ter despesas.</exception>
    public async Task<TransacaoDto> CriarAsync(string descricao, decimal valor, TipoTransacao tipo, int pessoaId)
    {
        if (string.IsNullOrWhiteSpace(descricao))
            throw new ArgumentException("Descrição é obrigatória.");

        if (valor <= 0)
            throw new ArgumentException("Valor deve ser maior que zero.");

        var pessoa = await pessoaRepository.ObterPorIdAsync(pessoaId);
        if (pessoa is null)
            throw new ArgumentException("Pessoa informada não existe.");

        if (pessoa.MenorIdade() && tipo == TipoTransacao.Receita)
            throw new InvalidOperationException("Pessoas menores de idade só podem cadastrar despesas.");

        var transacao = new Transacao
        {
            Descricao = descricao,
            Valor = valor,
            Tipo = tipo,
            PessoaId = pessoaId
        };

        await transacaoRepository.AdicionarAsync(transacao);
        await transacaoRepository.SalvarAsync();

        return TransacaoDto.FromEntity(transacao);
    }
}