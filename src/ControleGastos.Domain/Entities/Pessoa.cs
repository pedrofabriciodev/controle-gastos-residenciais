namespace ControleGastos.Domain;


/// <summary>
/// Representa uma pessoa cadastrada no sistema e as suas transações associadas.
/// </summary>
public class Pessoa
{
    /// <summary>
    /// Identificador de cada pessoa.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Nome da pessoa.
    /// </summary>
    public required string Nome { get; set; }
    
    /// <summary>
    /// Idade da pessoa.
    /// </summary>
    public int Idade { get; set; }

    /// <summary>
    /// Lista to tipo transacao, com todas as transações realizada por essa pessoa (seja receita ou despesa).
    /// </summary>
    public List<Transacao> Transacoes { get; set; } = new();
    
    /// <summary>
    /// Verifica se a pessoa é menor de idade (possui menos de 18 anos). 
    /// </summary>
    /// <returns>Retorna true se a idade da pessoa for menor que 18 e false caso a idade seja maior ou igual a 18.</returns>
    public bool MenorIdade() => Idade < 18;
}