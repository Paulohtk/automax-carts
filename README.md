# Automax Carts

Painel interno fullstack para consultar carrinhos da Fake Store API. O backend sincroniza os carrinhos para um banco SQLite local e o frontend React lista os dados armazenados.

## Tecnologias usadas

- Backend: C# / ASP.NET Core Web API
- Banco local: SQLite
- Persistencia: Entity Framework Core
- Frontend: ReactJS com Vite e TypeScript
- Testes: xUnit no backend, Vitest e Testing Library no frontend

## Estrutura de pastas

```text
backend/
  Automax.Api/
    Configuration/
    Controllers/
    Data/
    DTOs/
    Models/
    Repositories/
    Services/
  Automax.Tests/
frontend/
  src/
    components/
    pages/
    services/api/
    test/
    types/
docs/
  AI_USAGE.md
SPEC.md
README.md
```

## Como rodar o backend

```bash
dotnet restore
dotnet run --project backend/Automax.Api
```

A API sobe por padrao em `http://localhost:5007`.

## Como configurar o banco local

Nao ha passo manual. O SQLite usa a connection string em `backend/Automax.Api/appsettings.json`:

```json
"DefaultConnection": "Data Source=automax-carts.db"
```

O arquivo do banco e criado automaticamente na primeira execucao da API.

## Como sincronizar dados

Com o backend rodando, execute:

```bash
curl -X POST http://localhost:5007/carts/sync
```

Esse trigger consulta `https://fakestoreapi.com/carts`, salva os carrinhos no SQLite e retorna a quantidade sincronizada.

Tambem e possivel executar a sincronizacao pelo botao **Sincronizar dados** no frontend.

## Endpoints disponiveis

| Metodo | Rota | Descricao |
| --- | --- | --- |
| GET | `/carts` | Lista carrinhos armazenados. |
| GET | `/carts/{id}` | Busca um carrinho por id. |
| POST | `/carts/sync` | Sincroniza carrinhos da Fake Store API para o banco local. |

## Como rodar o frontend

```bash
cd frontend
npm install
npm run dev
```

O frontend sobe em `http://localhost:5173` e consome `http://localhost:5007` por padrao. Para alterar a URL da API, defina `VITE_API_BASE_URL`.

A tela principal lista os carrinhos armazenados e inclui os diferenciais opcionais permitidos no desafio:

- sincronizacao manual dos dados pela interface;
- filtro por `userId`;
- filtro por intervalo de datas.

## Como rodar os testes

Backend:

```bash
dotnet test
```

Frontend:

```bash
cd frontend
npm test
```

## Decisoes tecnicas

- Usei SQLite por ser local, simples e suficiente para o desafio.
- Usei Entity Framework Core para reduzir codigo de persistencia manual e manter o repository legivel.
- A sincronizacao ficou em `CartSyncService`, separada do controller, para manter regra de integracao fora da camada HTTP.
- O repository centraliza consultas e upsert dos carrinhos.
- O endpoint `POST /carts/sync` foi escolhido como trigger manual de sincronizacao, um dos diferenciais opcionais permitidos, sem scheduler para evitar complexidade desnecessaria.
- A quantidade total de produtos e calculada como soma de `quantity` dos produtos do carrinho.
- O frontend usa uma tabela simples com barra de filtros porque o objetivo e consulta interna, com foco em leitura rapida e baixa complexidade visual.

## SOLID e arquitetura

O backend foi organizado em camadas simples dentro de um unico projeto de API, com separacao clara entre `Controllers`, `Services`, `Repositories`, `Data`, `Models`, `DTOs` e `Configuration`. Essa abordagem foi escolhida para manter a entrega objetiva, legivel e proporcional ao escopo do desafio.

A implementacao aplica SOLID de forma pragmatica:

- `Single Responsibility Principle`: controllers tratam HTTP, services concentram regras de sincronizacao, repositories cuidam da persistencia e DTOs representam contratos de entrada/saida.
- `Dependency Inversion Principle`: o controller depende de abstracoes como `ICartRepository` e `ICartSyncService`, nao diretamente das implementacoes concretas.
- `Interface Segregation Principle`: as interfaces criadas sao pequenas e especificas para o caso de uso atual.
- `Open/Closed Principle`: a separacao por contratos permite trocar a implementacao de persistencia ou sincronizacao com baixo impacto nas camadas consumidoras.

A arquitetura e inspirada em Clean Architecture, mas nao foi implementada como Clean Architecture completa com projetos separados para Domain, Application, Infrastructure e Presentation. Essa decisao foi intencional: o desafio pede uma estrutura modular simples, e criar multiplos projetos para um fluxo pequeno poderia adicionar complexidade sem ganho real para a entrega.

Se o sistema evoluisse, a proxima etapa natural seria separar o backend em projetos como:

```text
Automax.Domain
Automax.Application
Automax.Infrastructure
Automax.Api
Automax.Tests
```

Para esta entrega, a escolha foi manter uma arquitetura modular, limpa e facil de demonstrar, respeitando o limite de nao adicionar recursos fora do documento.

## Spec Driven Development

A especificacao do desafio foi transformada em [SPEC.md](SPEC.md), contendo objetivo, requisitos, endpoints, modelo de dados, fluxo de sincronizacao e criterios de aceite.

## Uso de IA

O uso de IA esta documentado em [docs/AI_USAGE.md](docs/AI_USAGE.md), incluindo ferramenta, prompts, decisoes, correcoes e limitacoes.

## Pontos de melhoria

- Adicionar migracoes formais do EF Core caso o projeto evolua alem da entrega tecnica.
- Adicionar testes de integracao com host HTTP real caso a API evolua para mais endpoints.
