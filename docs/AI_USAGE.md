# Uso de IA no desenvolvimento

## Ferramenta usada

Codex.

## Por que foi usada

Foi usada como apoio de engenharia para transformar o desafio em especificacao, organizar a arquitetura, acelerar a implementacao fullstack, revisar escopo e documentar decisoes tecnicas. A IA nao substituiu a leitura do documento: ela foi usada para derivar tarefas a partir dos requisitos e para conferir se a entrega permanecia dentro do escopo.

## Prompts principais utilizados

- "Atue como engenheiro fullstack senior. A partir deste desafio, gere primeiro uma SPEC com requisitos funcionais, nao funcionais, endpoints, modelo de dados, fluxo de sincronizacao e criterios de aceite. Nao implemente nada fora do escopo obrigatorio, exceto os diferenciais opcionais explicitamente permitidos."
- "Com base na SPEC, proponha uma arquitetura simples para backend C# e frontend React. Evite overengineering: mantenha separacao clara entre controllers, services, repositories, data, models, DTOs e configuration, mas sem criar camadas ou recursos nao solicitados."
- "Implemente a API REST em ASP.NET Core com SQLite e Entity Framework Core. Consuma `GET https://fakestoreapi.com/carts`, persista Cart e CartProduct no banco local e exponha `GET /carts`, `GET /carts/{id}` e um trigger manual documentado para sincronizacao."
- "Implemente o frontend React como painel interno: tabela limpa, leitura rapida, consumo apenas do backend, filtros opcionais por userId e intervalo de datas, e acao de sincronizacao manual sem adicionar autenticacao, dashboard extra ou telas fora do desafio."
- "Revise a entrega contra a SPEC: verifique responsabilidades, SOLID pragmatico, testes basicos relevantes, documentacao de execucao, uso de IA, decisoes tecnicas e possiveis pontos de melhoria."

## Onde a IA ajudou

- Transformar o enunciado em uma especificacao objetiva antes da implementacao.
- Separar responsabilidades no backend sem criar complexidade desnecessaria.
- Criar o fluxo de sincronizacao com a Fake Store API e persistencia local.
- Montar uma interface React simples, moderna e focada na consulta interna.
- Escrever testes para repository, service, controller, renderizacao da lista, filtros e sincronizacao pela tela.
- Revisar documentacao para explicar escolhas tecnicas e limites do escopo.

## Sugestao inadequada da IA e correcao feita

A primeira tentativa de adicionar pacotes do Entity Framework Core selecionou a versao 10.x, incompativel com o alvo `net9.0` do projeto. A correcao foi fixar explicitamente os pacotes na versao `9.0.10`, mantendo compatibilidade com o SDK local.

Tambem houve cuidado para nao transformar o desafio em uma arquitetura maior que o necessario. Ideias como autenticacao, Docker, mensageria, cache, roles, dashboards adicionais e telas de detalhe foram descartadas porque nao fazem parte do documento.

## Decisoes tomadas apos revisao

- A sincronizacao ficou como trigger manual por endpoint e tambem por botao no frontend, alinhada ao diferencial opcional permitido.
- Os filtros por `userId` e intervalo de datas foram implementados no frontend, sem alterar o contrato obrigatorio do backend.
- A documentacao descreve a arquitetura como modular e inspirada em Clean Architecture, sem afirmar que se trata de Clean Architecture completa.
- Os testes foram ampliados para cobrir controller, fluxo de sincronizacao, repository, renderizacao, filtros e acao de sincronizar pela interface.

## Limitacoes conscientes

- Nao foram adicionadas migracoes versionadas do EF Core; `EnsureCreated` foi mantido para simplicidade local no contexto do desafio.
- Nao ha autenticacao ou autorizacao, pois o documento nao solicita esse requisito.
- Nao ha scheduler periodico em background; a sincronizacao manual foi escolhida por ser simples, demonstravel e permitida no enunciado.

## O que faria diferente com mais tempo

- Adicionaria migracoes formais para evolucao controlada do schema.
- Criaria testes de integracao HTTP ponta a ponta para a API.
- Melhoraria observabilidade com logs estruturados caso o painel virasse um sistema interno real.
