# SPEC - Automax Carts

## Objetivo do sistema

Criar uma aplicacao fullstack simples para que a Automax consulte carrinhos de compras vindos da Fake Store API, armazenados antes em um banco local.

## Requisitos funcionais

- Consumir carrinhos da Fake Store API em `GET https://fakestoreapi.com/carts`.
- Armazenar os carrinhos em banco local SQLite.
- Expor `GET /carts` para listar carrinhos armazenados.
- Expor `GET /carts/{id}` para consultar um carrinho armazenado por id.
- Exibir no frontend a lista de carrinhos armazenados.
- Exibir para cada carrinho: id, data de criacao, id do usuario e quantidade total de produtos.
- Permitir sincronizacao manual dos dados por trigger de execucao documentado.
- Permitir sincronizacao manual tambem pela interface do frontend.
- Permitir filtros no frontend por userId e intervalo de datas.

## Requisitos nao funcionais

- Backend em C# com API REST.
- Banco local SQLite.
- Organizacao modular no backend com Controllers, Services, Repositories, Data, Models/Entities, DTOs e Configuration.
- Frontend em ReactJS.
- Codigo simples, legivel e com responsabilidades separadas.
- Documentacao de execucao local, decisoes tecnicas e uso de IA.
- Testes basicos onde fizer sentido.

## Endpoints necessarios

| Metodo | Rota | Descricao |
| --- | --- | --- |
| GET | `/carts` | Lista carrinhos armazenados no SQLite. |
| GET | `/carts/{id}` | Retorna um carrinho armazenado por id. |
| POST | `/carts/sync` | Trigger manual para buscar dados da Fake Store API e salvar no SQLite. |

## Modelo de dados

### Cart

- `Id`: identificador do carrinho vindo da Fake Store API.
- `UserId`: identificador do usuario.
- `Date`: data de criacao do carrinho.
- `Products`: produtos do carrinho.

### CartProduct

- `Id`: identificador local.
- `CartId`: identificador do carrinho.
- `ProductId`: identificador do produto vindo da Fake Store API.
- `Quantity`: quantidade do produto no carrinho.

## Fluxo de sincronizacao com Fake Store API

1. O usuario ou demonstrador executa `POST /carts/sync`.
2. O backend consulta `GET https://fakestoreapi.com/carts`.
3. O service converte o retorno externo para entidades locais.
4. O repository faz upsert dos carrinhos e substitui os produtos relacionados para refletir a fonte externa.
5. Os endpoints `GET /carts` e `GET /carts/{id}` passam a retornar os dados do SQLite.

## Criterios de aceite

- O backend inicia localmente sem configuracoes externas obrigatorias.
- O SQLite e criado localmente na execucao da API.
- `POST /carts/sync` popula ou atualiza os carrinhos no banco local.
- `GET /carts` retorna a lista de carrinhos armazenados.
- `GET /carts/{id}` retorna `404` quando o carrinho nao existe.
- O frontend lista os carrinhos retornados pelo backend.
- A quantidade total de produtos exibida corresponde a soma das quantidades dos produtos do carrinho.
- O frontend filtra a lista por userId quando o filtro e preenchido.
- O frontend filtra a lista por data inicial e data final quando o intervalo e preenchido.
- O README explica execucao, estrutura, sincronizacao, decisoes tecnicas e uso de IA.
