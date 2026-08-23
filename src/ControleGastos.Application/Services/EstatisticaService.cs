using ControleGastos.Application.DTOs;
using ControleGastos.Application.Interfaces;
using ControleGastos.Domain;

namespace ControleGastos.Application.Services;

public class EstatisticaService(ITransacaoRepository transacaoRepository, IPessoaRepository pessoaRepository)
{
    public async Task<EstatisticasDto> ConsultarEstatisticasAsync()
    {
        var transacoes = await transacaoRepository.ListarTodasAsync();
        var pessoas = await pessoaRepository.ListarTodasAsync();

        decimal maior = decimal.MinValue;
        decimal menor = decimal.MaxValue;
        decimal soma = 0;
        int quantidadeReceitas = 0;
        int quantidadeDespesas = 0;

        foreach (var t in transacoes)
        {
            if (t.Valor > maior) maior = t.Valor;
            if (t.Valor < menor) menor = t.Valor;
            soma += t.Valor;

            if (t.Tipo == TipoTransacao.Receita) quantidadeReceitas++;
            else quantidadeDespesas++;
        }

        int quantidadeTotal = transacoes.Count;
        decimal media = quantidadeTotal > 0 ? soma / quantidadeTotal : 0;

        var ranking = new List<RankingPessoaDto>();
        foreach (var pessoa in pessoas)
        {
            decimal saldoPessoa = 0;
            foreach (var t in transacoes)
            {
                if (t.PessoaId != pessoa.Id) continue;
                saldoPessoa += t.Tipo == TipoTransacao.Receita ? t.Valor : -t.Valor;
            }
            ranking.Add(new RankingPessoaDto(pessoa.Nome, saldoPessoa));
        }

        for (int i = 0; i < ranking.Count - 1; i++)
        {
            for (int j = 0; j < ranking.Count - i - 1; j++)
            {
                if (ranking[j].Saldo < ranking[j + 1].Saldo)
                {
                    (ranking[j], ranking[j + 1]) = (ranking[j + 1], ranking[j]);
                }
            }
        }

        return new EstatisticasDto(
            quantidadeTotal > 0 ? maior : 0,
            quantidadeTotal > 0 ? menor : 0,
            media,
            quantidadeTotal,
            quantidadeReceitas,
            quantidadeDespesas,
            ranking);
    }
}