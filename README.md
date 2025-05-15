# 💸 WalletApi

API REST para gerenciamento de carteiras digitais, desenvolvida em C# com .NET 9, PostgreSQL, autenticação JWT e arquitetura limpa (Clean Architecture).

## 📚 Visão Geral

A WalletApi permite:

- Criar usuários
- Consultar e adicionar saldo à carteira
- Realizar transferências entre usuários
- Listar transferências com filtros por data
- Segurança via autenticação JWT

## ⚙️ Tecnologias

- [.NET 9](https://dotnet.microsoft.com)
- ASP.NET Core Web API
- PostgreSQL
- JWT (JSON Web Tokens)
- Entity Framework Core
- Docker e Docker Compose
- Arquitetura Limpa (Clean Architecture)

## 🚀 Como Executar
- Para rodar a aplicação dessa maneira tem que alterar o arquivo appsetting, na string de conexão, apontar o Host para local, comentar a aplicação no compose e subir só a base de dados.
```bash
dotnet run
```

### 🐳 Com Docker

```bash
docker-compose up --build
ou
docker-compose up -d 
```

### 💻 Localmente

1. Configure o banco PostgreSQL com os dados esperados (ou use Docker)
2. Atualize as strings de conexão em `appsettings.Development.json`
3. Rode a aplicação:

```bash
dotnet build
dotnet run --project src/WalletApi
```

## 🔐 Autenticação

A API utiliza JWT para autenticação. Após criar um usuário, você poderá gerar um token via login (caso disponível) ou manualmente para testes.

Para usar o token:

```
Authorization: Bearer {seu_token}
```

## 🔧 Endpoints Principais
- Via docker-compose
```http
http://localhost:8080
```
- Via aplicação direta
```http
http://localhost:5194
````
```https
https://localhost:7196
```

### Login

### Fazer login
```http
POST /v1/auth/login
Content-Type: application/json
```

```json
{
  "email": "john@example.com",
  "password": "Admin@123456",
}
```


### 👤 Usuários

#### Criar usuário

```http
POST /v1/api/users
Content-Type: application/json
```

```json
{
  "username": "John.Doe",
  "email": "john@example.com",
  "password": "Admin@123456",
  "ConfirmPassword": "Admin@2025"
}
```

### 💰 Saldo

#### Consultar saldo

```http
GET /v1/api/wallet
Authorization: Bearer {token}
```

#### Adicionar saldo

```http
POST /v1/api/wallet/deposit
Authorization: Bearer {token}
```

```json
{
  "amount": 100.0
}
```

### 🔁 Transferências

#### Criar transferência

```http
POST /v1/api/transfer
Authorization: Bearer {token}
```

```json
{
  "receiverUserId": "userId",
  "amount": 25.0
}
```

#### Listar transferências (com filtros)

```http
GET /v1/api/transfer?startDate=2024-01-01&endDate=2024-12-31
Authorization: Bearer {token}
```
ou 

```http
GET /v1/api/transfer
Authorization: Bearer {token}
```

## 🧱 Arquitetura

O projeto segue a Clean Architecture, separado em:

- **Domain** – Entidades e regras de negócio
- **Application** – Casos de uso (UseCases)
- **Infrastructure** – Repositórios, banco de dados
- **Presentation** – Controllers e endpoints da API

## 🧪 Testes

Para executar os testes:

```bash
dotnet test
```

## 📂 Estrutura do Projeto

```bash
WalletApi/
├── src/
│   ├── WalletApi/               # Camada Presentation
│   ├── WalletApplication/       # Camada Application
│   ├── WalletDomain/            # Camada Domain
│   └── WalletInfrastructure/    # Camada Infrastructure
├── docker-compose.yaml
├── WalletApi.sln
└── README.md
```

## 📫 Contato

Desenvolvido por [Thiago T Monteiro](https://github.com/ThiagoTMonteiro)  
Contribuições são bem-vindas!

