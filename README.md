# TaskFlow API

API REST para gerenciamento de tarefas, desenvolvida com C#, .NET 8, NUnit, TDD, Docker e GitHub Actions para disciplina da pós-graduação INFNET.

## Objetivo
Esse projeto é parte de uma atividade prática da disciplina de DevOps com objetivo de solidificar conhecimentos em:

- Desenvolvimento orientado a testes (TDD)
- Criação de testes automatizados com boa cobertura.
- Integração contínua usando **GitHub Actions**
- Uso da aplicação em container com Docker

## Tecnologias
- C#
- .NET 8
- ASP.NET Core
- NUnit
- Docker
- GitHub Actions
- Visual Studio Code

## Funcionalidades

A API permite:

- criar uma nova tarefa
- listar todas as tarefas
- buscar uma tarefa por ID
- atualizar título e descrição de uma tarefa
- marcar uma tarefa como concluída
- excluir uma tarefa

---

## Regras de negócio

As principais regras implementadas são:

- o título da tarefa é obrigatório
- não é permitido criar tarefa com título vazio
- não é permitido atualizar tarefa inexistente
- não é permitido concluir tarefa inexistente
- não é permitido excluir tarefa inexistente

---

## Estrutura do projeto

```text
taskflow-api/
├── .github/
│   └── workflows/
│       └── ci-cd.yml
├── src/
│   └── TaskFlow.Api/
│       ├── Contracts/
│       ├── Domain/
│       ├── Endpoints/
│       ├── Repositories/
│       └── Services/
├── tests/
│   └── TaskFlow.Api.Tests/
│       ├── Fakes/
│       ├── Integration/
│       └── Services/
├── Dockerfile
├── .dockerignore
├── .gitignore
├── README.md
└── TaskFlow.sln
```

## Abordagem TDD

O desenvolvimento foi guiado por TDD. Essa abordagem foi aplicada principalmente na camada de serviço, antes da exposição dos endpoints HTTP.

### Exemplos de ciclos implementados
- criação de tarefa
- listagem de tarefas
- busca por ID
- atualização de tarefa
- conclusão de tarefa
- exclusão de tarefa

## Testes automatizados

O projeto possui:

- **testes unitários**, focados na regra de negócio da TaskService
- **testes de integração**, validando os endpoints HTTP da API

### Exemplos de cenários testados
- criar tarefa com título válido
- impedir criação com título vazio
- listar tarefas cadastradas
- buscar tarefa existente e inexistente
- atualizar tarefa existente
- impedir atualização de tarefa inexistente
- concluir tarefa existente
- excluir tarefa existente
- validar respostas HTTP dos endpoints