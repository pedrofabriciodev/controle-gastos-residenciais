using ControleGastos.Domain;

namespace ControleGastos.Application.DTOs;

/// <summary>
/// Representa uma transação para retorno via API.
/// </summary>
/// <param name="Id">Identificador único da transação.</param>
/// <param name="Descricao">Descrição informada para a transação.</param>
/// <param name="Valor">Valor monetário da transação.</param>
/// <param name="Tipo">Tipo da transação: Receita ou Despesa.</param>
/// <param name="PessoaId">Identificador único da pessoa dona dessa transação.</param>
public record TransacaoDto(int Id, string Descricao, decimal Valor, TipoTransacao Tipo, int PessoaId)
{
    public static TransacaoDto FromEntity(Transacao t) =>
        new(t.Id, t.Descricao, t.Valor, t.Tipo, t.PessoaId);
}