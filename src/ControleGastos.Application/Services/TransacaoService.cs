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
    public async Task<List<TransacaoDto>> BuscarPorDescricaoAsync(string termo)
    {
        var todas = await transacaoRepository.ListarTodasAsync();
        var resultado = new List<Transacao>();

        foreach (var t in todas)
        {
            if (t.Descricao.Contains(termo, StringComparison.OrdinalIgnoreCase))
                resultado.Add(t);
        }

        return resultado.Select(TransacaoDto.FromEntity).ToList();
    }
    
    public async Task<List<TransacaoDto>> OrdenarPorValorAsync(bool ascendente = true)
    {
        var lista = (await transacaoRepository.ListarTodasAsync()).ToList();
        int n = lista.Count;

        //Bubble sort
        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                bool foraDeOrdem = ascendente
                    ? lista[j].Valor > lista[j + 1].Valor
                    : lista[j].Valor < lista[j + 1].Valor;

                if (foraDeOrdem)
                {
                    (lista[j], lista[j + 1]) = (lista[j + 1], lista[j]);
                }
            }
        }

        return lista.Select(TransacaoDto.FromEntity).ToList();
    }
}