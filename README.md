# Controle de Gastos Residenciais

Sistema de controle de gastos com cadastro de pessoas, registro de transações financeiras (receitas e despesas) e consulta de totais consolidados por pessoa e geral.

## Visão geral

A aplicação permite:

- **Cadastrar pessoas**, com identificador único gerado automaticamente, nome e idade;
- **Registrar transações financeiras** (receitas ou despesas) vinculadas a uma pessoa já cadastrada;
- **Consultar os totais** de receitas, despesas e saldo de cada pessoa, além do total geral consolidado de todas as pessoas do sistema.

Uma regra de negócio central do sistema é que **pessoas menores de idade (menos de 18 anos) só podem ter despesas cadastradas** (receitas são bloqueadas para esse perfil).

## Tecnologias utilizadas

**Backend**
- .NET 9 / C#
- ASP.NET Core Web API
- Entity Framework Core
- SQLite (persistência em arquivo, sem necessidade de servidor de banco de dados separado)
- Swagger / OpenAPI (documentação interativa da API)

**Frontend**
- React
- TypeScript
- Vite
- Tailwind CSS

## Arquitetura

O backend segue uma separação em camadas inspirada em Clean Architecture, dividida em quatro projetos:

```
Domain          → Entidades e regras de negócio intrínsecas ao domínio (Pessoa, Transacao)
Application     → Casos de uso, DTOs e interfaces de repositório
Infrastructure  → Implementação de acesso a dados (EF Core, DbContext, repositórios)
Api             → Controllers, configuração da aplicação (Program.cs) e ponto de entrada
```

Cada Controller delega toda a lógica de negócio para um Service correspondente; 

Os Services usam interfaces de repositório para acessar dados, sem conhecer detalhes de implementação do EF Core.

## Regras de negócio

- O identificador de Pessoa e de Transação é gerado automaticamente pelo banco de dados, nunca informado pelo cliente.
- Ao remover uma pessoa, todas as transações vinculadas a ela são removidas automaticamente (cascade delete configurado no banco).
- Toda transação precisa ser vinculada a uma pessoa já existente no sistema.
- Pessoas menores de idade (idade menor que 18 anos) só podem ter transações do tipo **Despesa** cadastradas, uma tentativa de cadastrar **Receita** para uma pessoa menor de idade é rejeitada pela API.
- A consulta de totais exibe, para cada pessoa, o total de receitas, total de despesas e o saldo (receitas menos despesas), além do total geral somando todas as pessoas cadastradas.

## Como rodar o projeto

### Pré-requisitos

- [.NET SDK 9](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) (versão 18 ou superior)
- [dotnet-ef](https://learn.microsoft.com/ef/core/cli/dotnet) (ferramenta de linha de comando do Entity Framework Core)

Para instalar o `dotnet-ef`, caso ainda não tenha:

```bash
dotnet tool install --global dotnet-ef --version 9.0.0
```

### Backend

1. Na raiz do repositório, restaure as dependências e aplique as migrations para criar o banco de dados local:

```bash
dotnet restore
dotnet ef database update --project src/ControleGastos.Infrastructure --startup-project src/ControleGastos.Api
```

Isso cria o arquivo `controlegastos.db` (SQLite) dentro de `src/ControleGastos.Api`, já com o esquema de tabelas necessário.

2. Rode a API:

```bash
dotnet run --project src/ControleGastos.Api
```

A API sobe por padrão em uma porta local exibida no terminal (ex.: `http://localhost:5062`). A documentação interativa (Swagger) fica disponível em `http://localhost:<porta>/swagger`.

### Frontend

1. Instale as dependências:

```bash
cd frontend
npm install
```

2. Rode a aplicação:

```bash
npm run dev
```

Por padrão, o Vite sobe o frontend em `http://localhost:5173`.

> **Importante:** a URL da API está configurada em `frontend/src/services/api.ts`. Caso a porta da API exibida no terminal seja diferente da configurada, ajuste essa constante antes de rodar o frontend.

## Endpoints da API

| Método | Rota                | Descrição                                              |
|--------|----------------------|---------------------------------------------------------|
| GET    | `/api/Pessoas`       | Lista todas as pessoas cadastradas                      |
| POST   | `/api/Pessoas`       | Cadastra uma nova pessoa                                 |
| DELETE | `/api/Pessoas/{id}`  | Remove uma pessoa e suas transações vinculadas           |
| GET    | `/api/Transacoes`    | Lista todas as transações cadastradas                     |
| POST   | `/api/Transacoes`    | Cadastra uma nova transação                                |
| GET    | `/api/Totais`        | Consulta os totais de receitas, despesas e saldo          |

### Exemplo de payload — criar pessoa

```json
{
  "nome": "Maria Silva",
  "idade": 25
}
```

### Exemplo de payload — criar transação

```json
{
  "descricao": "Compra no supermercado",
  "valor": 150.50,
  "tipo": "Despesa",
  "pessoaId": 1
}
```

O campo `tipo` aceita os valores `"Receita"` ou `"Despesa"`.

## Estrutura de pastas

```
.
├── ControleGastos.sln
├── src/
│   ├── ControleGastos.Domain/          # Entidades (Pessoa, Transacao) e regras intrínsecas ao domínio
│   ├── ControleGastos.Application/     # Services, DTOs e interfaces de repositório
│   ├── ControleGastos.Infrastructure/  # DbContext, migrations e implementação dos repositórios
│   └── ControleGastos.Api/             # Controllers e configuração da aplicação
├── tests/
│   └── ControleGastos.Tests/
└── frontend/
    └── src/
        ├── components/                 # Componentes React (formulários, listagens, resumo financeiro)
        ├── services/                    # Camada de comunicação com a API
        └── types/                        # Interfaces TypeScript espelhando os DTOs da API
```

## Decisões técnicas

- **SQLite** foi escolhido para persistência por não exigir um servidor de banco de dados separado
- **DTOs de resposta** (`TransacaoDto`, `TotalGeralDto`) são utilizados em vez de retornar as entidades de domínio diretamente pela API, evitando expor detalhes internos do modelo e problemas de serialização decorrentes de referências circulares entre entidades relacionadas (Pessoa ↔ Transação).
- **Separação em camadas** (Domain, Application, Infrastructure, Api) foi adotada para isolar regras de negócio de detalhes de infraestrutura, facilitando manutenção e possibilitando testes unitários independentes de banco de dados.
- **Injeção de dependência via interfaces de repositório** permite trocar a implementação de persistência sem alterar a camada de regras de negócio.

## Possíveis melhorias futuras

- Cobertura de testes unitários e de integração para as regras de negócio centrais.
- Paginação na listagem de transações para bases de dados maiores.
- Autenticação e autorização de usuários.