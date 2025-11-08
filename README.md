# 🚀 SysTrack

**SysTrack** é uma aplicação desenvolvida em **ASP.NET Core Web API** para gerenciar pátios de veículos e suas respectivas motocicletas, oferecendo filtros personalizados, paginação, ordenação e previsões de manutenção com ML.NET, garantindo maior eficiência na administração da frota.

## 📌 Índice

- [🧾 Sobre o Projeto](#-sobre-o-projeto)
- [⚙️ Tecnologias Utilizadas](#-tecnologias-utilizadas)
- [🧪 Como Executar](#-como-executar)
- [🧪 Testes](#-testes)
- [📌 Endpoints da API](#-endpoints-da-api)
- [✅ Funcionalidades](#-funcionalidades)
- [🗃️ Modelo de Dados](#-modelo-de-dados)
- [👨‍💻 Nossa equipe](#-nossa-equipe)

---

## 🧾 Sobre o Projeto

O objetivo do SysTrack é fornecer uma **API RESTful robusta** para cadastro, listagem, filtragem e manutenção de pátios e motocicletas, com:

- Validações e regras de negócio bem definidas.
- Previsão de necessidade de manutenção usando **ML.NET**.
- Controle de acesso via **API Key**.
- Versionamento de API.
- Health checks para monitoramento de serviços.

A aplicação segue boas práticas do **ASP.NET Core Web API**, utilizando **DTOs**, **Controllers**, **Services** e **Swagger** para documentação interativa.

---

## ⚙️ Tecnologias Utilizadas

- C#  
- .NET 6 ou superior  
- ASP.NET Core Web API  
- Entity Framework Core (EF Core)  
- Oracle Database  
- Oracle.EntityFrameworkCore  
- Swagger/OpenAPI  
- ML.NET (para previsão de manutenção)  
- xUnit (para testes unitários e de integração)  

---

## 🧪 Como Executar

### Ambiente

- .NET SDK 7.0 ou superior  
- Oracle Database  
- Visual Studio 2022+ ou Visual Studio Code  
- dotnet ef  
- Postman ou outro programa de testes de API  

### Pacotes NuGet importantes

- Microsoft.EntityFrameworkCore  
- Microsoft.EntityFrameworkCore.Tools  
- Microsoft.EntityFrameworkCore.Design  
- Oracle.EntityFrameworkCore  
- Microsoft.ML  
- Microsoft.AspNetCore.Mvc.Testing  
- xUnit  

### Passos

```bash
# Clone o repositório
git clone https://github.com/guurangel/SysTrack-.net.git

# Acesse a pasta do projeto
cd SysTrack-.net

# Configure a string de conexão Oracle dentro de appsettings.json
"ConnectionStrings": {
  "Oracle": "User Id=SEU_USUARIO;Password=SUA_SENHA;Data Source=SEU_HOST:PORTA/SEU_SERVICE_NAME"
}

# Gerar e aplicar as migrations
dotnet ef migrations add CreateDatabase --context SysTrackDbContext
dotnet ef database update --context SysTrackDbContext

# Executar a aplicação
dotnet run


```

A API estará disponível em:  
📍 `http://localhost:5004`

Acesse o Swagger para testar os endpoints:  
📘 `http://localhost:5004/swagger/index.html`

---

## 🧪 Testes

### Testes unitários

- Localizados no projeto `SysTrack.Tests`.  
- Cobrem a lógica principal, como regras de negócios e cálculo de idade da motocicleta.  
- Executar pelo Visual Studio Test Explorer ou linha de comando:

```bash
dotnet test
```

Testes de integração

Realizam chamadas HTTP para endpoints reais da API.

Utilizam WebApplicationFactory<Program> para simular o host da aplicação.

Endpoints testados: /api/motocicleta/prever-manutencao.

Observação: Não é necessário ter a API rodando separadamente; o WebApplicationFactory inicializa a aplicação internamente para testes.

---

## 📌 Endpoints da API

### 🏍️ Motocicleta

- `GET /api/v1/motocicleta` — Lista todas as motos (com filtros e paginação)
- `POST /api/v1/motocicleta` — Cadastra uma nova moto
- `GET /api/v1/motocicleta/{id}` — Busca por ID
- `PUT /api/v1/motocicleta/{id}` — Atualiza dados
- `DELETE /api/v1/motocicleta/{id}` — Remove uma moto
-  `POST /api/v1/motocicleta/prever-manutencao` — Prediz se a moto necessita manutenção (ML.NET)

**Filtros disponíveis (como parâmetros da URL):**

- `placa`
- `marca`
- `modelo`
- `cor`
- `patioId` — buscar pelo pátio que a moto esta associada
- `Ano` — 
- `AnoInicio` — utilizar junto com AnoFim para intervalo de anos.
- `AnoFim`
- `Quilometragem` 
- `QuilometragemMin` — utilizar junto com QuilometragemMax para intervalo de quilometragem.
- `QuilometragemMax`
- `status` — FUNCIONAL ou MANUTENCAO

---

### 🏗️ Patio

- `GET /api/v1/patio` — Lista pátios (com filtros e paginação)
- `POST /api/v1/patio` — Cadastra um novo pátio
- `GET /api/v1/patio/{id}` — Detalha pátio
- `PUT /api/v1/patio/{id}` — Atualiza pátio
- `DELETE /api/v1/patio/{id}` — Remove pátio

---

### 👤 Usuário

- `GET /api/v1/usuario` — Lista todos os usuários (com filtros e paginação)
- `POST /api/v1/usuario` — Cadastra um novo usuário
- `GET /api/v1/usuario/{id}` — Busca por ID
- `PUT /api/v1/usuario/{id}` — Atualiza dados
- `DELETE /api/v1/usuario/{id}` — Remove um usuário

**Filtros disponíveis (como parâmetros da URL):**

- `nome` — buscar por parte do nome
- `email` — buscar por parte do e-mail
- `cpf` — buscar por parte do CPF
- `cargo` — filtrar por cargo do usuário
- `patioId` — buscar pelo pátio associado
- `dataAdmissaoInicio` — usuários admitidos a partir desta data
- `dataAdmissaoFim` — usuários admitidos até esta data

**Paginação:**

- `pageNumber` — número da página (default: 1)
- `pageSize` — quantidade de registros por página (default: 10)

---

### 👤 Health Check

- `GET api/v1/health` — Verifica a saúde do serviço e conexão com o banco de dados.

---

### 🔹 Autenticação via API Key

Todos os endpoints exigem um cabeçalho X-API-KEY.

Middleware customizado valida a chave antes de permitir acesso.

**Como usar:**

1. Adicione um cabeçalho `X-API-KEY` em todas as requisições HTTP.  
2. O valor do cabeçalho deve ser a chave definida no arquivo `appsettings.json` ou via variável de ambiente `API_KEY`.

---

### 🔹 Versionamento da API

Default: v1.

API versioning habilitado e relatado nos headers da resposta.

---

## ✅ Funcionalidades

- 🧱 Organização em camadas (Controllers, DTO, Infrastructure, Services).
- :file_cabinet: Utilizaçao de Migrations para criação da estrutura do banco de dados.
- 📖 Validações detalhadas com mensagens amigáveis.
- 📊 Documentação interativa via Swagger.
- 🏍️ Previsão de manutenção com ML.NET.
- 🔒 Proteção via API Key.
- 🧪 Testes unitários e de integração com xUnit.
- 📦 Paginação e ordenação nos endpoints.
- ⚡ Health checks para monitoramento da API.

---

## 🗃️ Modelo de Dados

### Motocicleta

```
Id: Guid
Placa: String
Marca: String
Modelo: String
Cor: String
DataEntrada: DateTime
Ano: Int
Quilometragem: Int
Status: Status
Patio: Patio
```

### Patio

```
Id: Guid
Nome: String
Endereco: String
CapacidadeMaxima: Int
DataCriacao: DateTime
Motocicleta: List
```

---

### Usuario

```
Id: Guid
Nome: String
Email: String
Senha: String
Cpf: String
Cargo: Cargo
Patio: Patio
```

---

## 👨‍💻 Nossa equipe

**Gustavo Rangel**  
💼 Estudante de Análise e Desenvolvimento de Sistemas na FIAP  
🔗 [linkedin.com/in/gustavoorangel](https://www.linkedin.com/in/gustavoorangel)

**David Rapeckman**  
💼 Estudante de Análise e Desenvolvimento de Sistemas na FIAP  
🔗 [linkedin.com/in/davidrapeckman](https://www.linkedin.com/in/davidrapeckman)

**Luis Felippe Morais**  
💼 Estudante de Análise e Desenvolvimento de Sistemas na FIAP  
🔗 [linkedin.com/in/luis-felippe-morais-das-neves-16219b2b9](https://www.linkedin.com/in/luis-felippe-morais-das-neves-16219b2b9)
