namespace ControleGastos.Application.DTOs;

public record RankingPessoaDto(string Nome, decimal Saldo);

public record EstatisticasDto(
    decimal MaiorValor,
    decimal MenorValor,
    decimal MediaValor,
    int QuantidadeTotal,
    int QuantidadeReceitas,
    int QuantidadeDespesas,
    List<RankingPessoaDto> RankingPorSaldo
);