namespace ControleGastos.Application.DTOs;

/// <summary>
/// Representa o resumo financeiro de uma pessoa: total de receitas, despesas e o saldo resultante.
/// </summary>
/// <param name="PessoaId">Identificador Único de cada pessoa.</param>
/// <param name="Nome">Nome da pessoa.</param>
/// <param name="TotalReceitas">Total das receitas dessa pessoa.</param>
/// <param name="TotalDespesas">Total das despesas dessa pessoa.</param>
/// <param name="Saldo">Resultado do total de despesas e receitas.</param>
public record TotalPessoaDto(int PessoaId, string Nome, decimal TotalReceitas, decimal TotalDespesas, decimal Saldo);

/// <summary>
/// Representa o resumo financeiro geral do sistema: o total de receitas, despesas e saldo somando todas as pessoas cadastradas, além do detalhamento individual de cada uma.
/// </summary>
/// <param name="PorPessoa">Lista com o resumo financeiro de cada pessoa cadastrada.</param>
/// <param name="TotalReceitas">Soma das receitas de todas as pessoas.</param>
/// <param name="TotalDespesas">Soma das despesas de todas as pessoas.</param>
/// <param name="Saldo">Resultado de TotalReceitas menos TotalDespesas, considerando todas as pessoas.</param>
public record TotalGeralDto(List<TotalPessoaDto> PorPessoa, decimal TotalReceitas, decimal TotalDespesas, decimal Saldo);