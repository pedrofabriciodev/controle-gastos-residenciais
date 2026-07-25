using ControleGastos.Application.DTOs;
using ControleGastos.Application.Interfaces;
using ControleGastos.Domain;

namespace ControleGastos.Application.Services;

/// <summary>
/// Serviço responsável por calcular os totais financeiros do sistema.
/// </summary>
/// <param name="pessoaRepository">Contrato de acesso a dados de Pessoa.</param>
/// <param name="transacaoRepository">Contrato de acesso a dados de Transação.</param>
public class TotalService(IPessoaRepository pessoaRepository, ITransacaoRepository transacaoRepository)
{
    /// <summary>
    /// Consulta todas as pessoas e transações cadastradas e calcula, para cada pessoa, o total de receitas, o total de despesas e o saldo.
    /// </summary>
    /// <returns>O detalhamento financeiro de cada pessoa, junto com o total geral somando todas elas.</returns>
    public async Task<TotalGeralDto> ConsultarTotaisAsync()
    {
        var pessoas = await pessoaRepository.ListarTodasAsync();
        var transacoes = await transacaoRepository.ListarTodasAsync();

        var totaisPorPessoa = pessoas.Select(pessoa =>
        {
            var transacoesDaPessoa = transacoes.Where(t => t.PessoaId == pessoa.Id);

            var totalReceitas = transacoesDaPessoa
                .Where(t => t.Tipo == TipoTransacao.Receita)
                .Sum(t => t.Valor);

            var totalDespesas = transacoesDaPessoa
                .Where(t => t.Tipo == TipoTransacao.Despesa)
                .Sum(t => t.Valor);

            return new TotalPessoaDto(pessoa.Id, pessoa.Nome, totalReceitas, totalDespesas, totalReceitas - totalDespesas);
        }).ToList();

        var totalGeralReceitas = totaisPorPessoa.Sum(p => p.TotalReceitas);
        var totalGeralDespesas = totaisPorPessoa.Sum(p => p.TotalDespesas);

        return new TotalGeralDto(
            totaisPorPessoa,
            totalGeralReceitas,
            totalGeralDespesas,
            totalGeralReceitas - totalGeralDespesas);
    }
}