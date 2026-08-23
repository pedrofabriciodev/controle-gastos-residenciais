using ControleGastos.Application.Services;
using ControleGastos.Domain;
using ControleGastos.Infrastructure;
using ControleGastos.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var optionsBuilder = new DbContextOptionsBuilder<ControleGastosDbContext>();
optionsBuilder.UseSqlite("Data Source=../ControleGastos.Api/controlegastos.db");

using var context = new ControleGastosDbContext(optionsBuilder.Options);

var pessoaRepository = new PessoaRepository(context);
var transacaoRepository = new TransacaoRepository(context);

var pessoaService = new PessoaService(pessoaRepository);
var transacaoService = new TransacaoService(transacaoRepository, pessoaRepository);
var estatisticaService = new EstatisticaService(transacaoRepository, pessoaRepository);

bool executando = true;

while (executando)
{
    Console.WriteLine();
    Console.WriteLine("=== Controle de Gastos Residenciais ===");
    Console.WriteLine("1 - Cadastrar Pessoa");
    Console.WriteLine("2 - Cadastrar Transação");
    Console.WriteLine("3 - Listar Pessoas e Transações");
    Console.WriteLine("4 - Buscar Transação por Descrição");
    Console.WriteLine("5 - Ordenar Transações");
    Console.WriteLine("6 - Mostrar Estatísticas");
    Console.WriteLine("7 - Sair");
    Console.Write("Escolha uma opção: ");

    var opcao = Console.ReadLine();

    switch (opcao)
    {
        case "1":
            await CadastrarPessoa();
            break;
        case "2":
            await CadastrarTransacao();
            break;
        case "3":
            await Listar();
            break;
        case "4":
            await Buscar();
            break;
        case "5":
            await Ordenar();
            break;
        case "6":
            await MostrarEstatisticas();
            break;
        case "7":
            executando = false;
            break;
        default:
            Console.WriteLine("Opção inválida.");
            break;
    }
}

async Task CadastrarPessoa()
{
    Console.Write("Nome: ");
    var nome = Console.ReadLine() ?? "";

    Console.Write("Idade: ");
    int.TryParse(Console.ReadLine(), out int idade);
    
    Console.Write("Email: ");
    string email = Console.ReadLine() ?? null;
    
    try
    {
        var pessoa = await pessoaService.CriarAsync(nome, idade, email);
        Console.WriteLine($"Pessoa cadastrada com Id {pessoa.Id}.");
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"Erro: {ex.Message}");
    }
}

async Task CadastrarTransacao()
{
    Console.Write("Descrição: ");
    var descricao = Console.ReadLine() ?? "";

    Console.Write("Valor: ");
    decimal.TryParse(Console.ReadLine(), out decimal valor);

    Console.Write("Tipo (Receita/Despesa): ");
    var tipoTexto = Console.ReadLine() ?? "Despesa";
    var tipo = tipoTexto.Equals("Receita", StringComparison.OrdinalIgnoreCase)
        ? TipoTransacao.Receita
        : TipoTransacao.Despesa;

    Console.Write("Id da Pessoa: ");
    int.TryParse(Console.ReadLine(), out int pessoaId);

    try
    {
        var transacao = await transacaoService.CriarAsync(descricao, valor, tipo, pessoaId);
        Console.WriteLine($"Transação cadastrada com Id {transacao.Id}.");
    }
    catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
    {
        Console.WriteLine($"Erro: {ex.Message}");
    }
}

async Task Listar()
{
    var pessoas = await pessoaService.ListarTodasAsync();
    Console.WriteLine("\n--- Pessoas ---");
    foreach (var p in pessoas)
        Console.WriteLine($"[{p.Id}] {p.Nome} - {p.Idade} anos");

    var transacoes = await transacaoService.ListarTodasAsync();
    Console.WriteLine("\n--- Transações ---");
    foreach (var t in transacoes)
        Console.WriteLine($"[{t.Id}] {t.Descricao} - R$ {t.Valor:F2} ({t.Tipo}) - Pessoa {t.PessoaId}");
}

async Task Buscar()
{
    Console.Write("Termo de busca: ");
    var termo = Console.ReadLine() ?? "";

    var resultado = await transacaoService.BuscarPorDescricaoAsync(termo);
    Console.WriteLine($"\n{resultado.Count} resultado(s) encontrado(s):");
    foreach (var t in resultado)
        Console.WriteLine($"[{t.Id}] {t.Descricao} - R$ {t.Valor:F2} ({t.Tipo})");
}

async Task Ordenar()
{
    Console.Write("Critério (valor/descricao): ");
    var criterio = Console.ReadLine() ?? "valor";

    Console.Write("Ordem (asc/desc): ");
    var ordem = Console.ReadLine() ?? "asc";
    bool ascendente = ordem.Equals("asc", StringComparison.OrdinalIgnoreCase);

    var resultado = await transacaoService.OrdenarAsync(criterio, ascendente);
    Console.WriteLine("\n--- Transações Ordenadas ---");
    foreach (var t in resultado)
        Console.WriteLine($"[{t.Id}] {t.Descricao} - R$ {t.Valor:F2} ({t.Tipo})");
}

async Task MostrarEstatisticas()
{
    var stats = await estatisticaService.ConsultarEstatisticasAsync();

    Console.WriteLine("\n--- Estatísticas ---");
    Console.WriteLine($"Maior valor: R$ {stats.MaiorValor:F2}");
    Console.WriteLine($"Menor valor: R$ {stats.MenorValor:F2}");
    Console.WriteLine($"Média dos valores: R$ {stats.MediaValor:F2}");
    Console.WriteLine($"Quantidade total de transações: {stats.QuantidadeTotal}");
    Console.WriteLine($"Receitas: {stats.QuantidadeReceitas} | Despesas: {stats.QuantidadeDespesas}");

    Console.WriteLine("\nRanking de pessoas por saldo:");
    foreach (var r in stats.RankingPorSaldo)
        Console.WriteLine($"{r.Nome}: R$ {r.Saldo:F2}");
}