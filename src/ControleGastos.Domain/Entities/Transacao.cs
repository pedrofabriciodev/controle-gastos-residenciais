namespace ControleGastos.Domain;

/// <summary>
/// Representa uma transação cadastrada no sistema e a sua pessoa associada, pois, toda transação é associada a uma pessoa.
/// </summary>
public class Transacao
{
    /// <summary>
    /// Identificador único, gerado automaticamente pelo banco de dados.
    /// </summary>
    public int Id {get; set;}
    
    /// <summary>
    /// Descrição obrigatória da transação.
    /// </summary>
    public required string Descricao {get; set;}
    
    /// <summary>
    /// Valor monetário da transação. Deve ser maior que zero.
    /// </summary>
    public decimal Valor {get; set;}
    
    /// <summary>
    /// Tipo obrigatório da transação.
    /// </summary>
    public required TipoTransacao Tipo {get; set;}
    
    /// <summary>
    /// Chave estrangeira para a pessoa dona dessa transação.
    /// </summary>
    public int PessoaId {get; set;}
    
    /// <summary>
    /// Propriedade para acesso aos dados da entidade pessoa, que efetua a trasanção.
    /// Importante para caso queira mostrar o nome de quem fez a transação ou algum outro dado.
    /// </summary>
    public Pessoa? Pessoa {get; set;}
    
    
}