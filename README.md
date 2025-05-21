# 🚀 SysTrack

**SysTrack** é uma aplicação desenvolvida em **ASP.NET Core Web API** para gerenciar pátios de veículos e suas respectivas motocicletas, oferecendo filtros personalizados, paginação e ordenação de dados para facilitar a administração de frota.

## 📌 Índice

- [🧾 Sobre o Projeto](#-sobre-o-projeto)
- [⚙️ Tecnologias Utilizadas](#-tecnologias-utilizadas)
- [🧪 Como Executar](#-como-executar)
- [📌 Endpoints da API](#-endpoints-da-api)
- [✅ Funcionalidades](#-funcionalidades)
- [🗃️ Modelo de Dados](#-modelo-de-dados)
- [👨‍💻 Nossa equipe](#-nossa-equipe)

---

## 🧾 Sobre o Projeto

O objetivo do **SysTrack** é fornecer uma API RESTful robusta para cadastro, listagem e filtragem de **pátios** e **motocicletas**, com validações e regras de negócio bem definidas. A aplicação é organizada seguindo boas práticas do ASP.NET Core Web API, com uso de Filters para filtros dinâmicos, DTOs para abstração de dados, e integração com o Swagger.

---

## ⚙️ Tecnologias Utilizadas

- C#
- .NET 6 ou superior 
- ASP.NET Core Web API
- Entity Framework Core (EF Core)
- Oracle Database
- Oracle.EntityFrameworkCore
- Swagger/OpenAPI

---

## 🧪 Como Executar

### Ambiente

- .NET SDK 7.0 ou superior
- Oracle Database
- Visual Studio 2022+ ou Visual Studio Code
- dotnet ef
- Postman ou outro programa de testes de API.

### Pacotes NuGet importantes

- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.Tools
- Microsoft.EntityFrameworkCore.Design
- Oracle.EntityFrameworkCore

### Passos

```bash
# Clone o repositório
git clone https://github.com/guurangel/SysTrack-.net.git

# Acesse a pasta do projeto
cd SysTrack-.net

# Configure a string de conexão Oracle
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
📘 `http://localhost:5004/swagger-ui.html`

---

## 📌 Endpoints da API

### 🏍️ Motocicleta

- `GET /api/motocicleta` — Lista todas as motos (com filtros e paginação)
- `POST /api/motocicleta` — Cadastra uma nova moto
- `GET /api/motocicleta/{id}` — Busca por ID
- `PUT /api/motocicleta/{id}` — Atualiza dados
- `DELETE /api/motocicleta/{id}` — Remove uma moto

**Filtros disponíveis (como parâmetros da URL):**

- `placa`
- `marca`
- `modelo`
- `cor`
- `patioId` — buscar pelo pátio que a moto esta associada

---

### 🏗️ Patio

- `GET /api/patio` — Lista pátios (com filtros e paginação)
- `POST /api/patio` — Cadastra um novo pátio
- `GET /api/patio/{id}` — Detalha pátio
- `PUT /api/patio/{id}` — Atualiza pátio
- `DELETE /api/patio/{id}` — Remove pátio

---

## ✅ Funcionalidades

- 🧱 Organização em camadas (Controllers, DTO, Infrastructure)
- :file_cabinet: Utilizaçao de Migrations para criação da estrutura do banco de dados
- 📖 Validações detalhadas com mensagens amigáveis
- 📊 Documentação interativa via Swagger
- 📦 Paginação e ordenação nos endpoints

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
Patio: Patio
```

### Patio

```
Id: Guid
Nome: String
Endereco: String
DataCriacao: DateTime
Motocicleta: List
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
