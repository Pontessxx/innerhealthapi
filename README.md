# 🌿 InnerHealth API

### Projeto Global Solution — SOA & WebServices (2º Semestre / FIAP)

---

## 📌 Visão Geral

A **InnerHealth API** é uma aplicação **RESTful** desenvolvida em **C#/.NET 8** com enfoque em **SOA – Arquitetura Orientada a Serviços**.
O objetivo é monitorar hábitos essenciais de bem-estar — como hidratação, sono, meditação, exposição ao sol, tarefas e atividades físicas — oferecendo dados consistentes e centralizados para gerar insights de saúde e produtividade.

O projeto foi desenvolvido seguindo rigorosamente os critérios da disciplina de **SOA & WebServices**, incluindo:

* Entities + DTOs + Enums
* Padrão ResponseEntity
* Tratamento global de exceções via ControllerAdvice
* Serviços independentes
* Modularização orientada a serviços
* Versionamento de API (v1 e v2)
* Documentação Swagger
* Autenticação com JWT
* Autorização por perfis de usuário
* Política de sessão **STATELESS**

---

## 👥 Integrantes do Grupo

| Nome                     | RM      |
| ------------------------ | ------- |
| Henrique Pontes Oliveira | RM98036 |
| Rafael Autieri dos Anjos | RM550885 |
| Rafael Carvalho Mattos | RM99874 |

---

## 🚀 Tecnologias Utilizadas

* **C# / .NET 8**
* **Entity Framework Core**
* **SQLite** (persistência automática por arquivo)
* **JWT Authentication**
* **BCrypt (hash de senha)**
* **Swagger / OpenAPI 3.0**
* **API Versioning**
* **Arquitetura baseada em serviços**

---

# 🏛 Arquitetura da Solução

A aplicação segue o padrão:

```
Cliente → Controllers → Services → Repositories → EF Core → SQLite
```

### ✔ Controllers

Recebem as requisições HTTP e delegam regras de negócio.

### ✔ Services

Contêm toda a lógica da aplicação.

### ✔ Repositories

Responsáveis pela persistência (CRUD).

### ✔ DTOs

Controlam entrada e saída de dados.

### ✔ Entities

Representam o modelo de domínio.

### ✔ GlobalExceptionHandler

Padroniza todos os erros retornados pela API.

---

## 📂 Estrutura de Pastas

```
InnerHealth.Api/
 ├── Auth/
 ├── Controllers/
 ├── Entities/
 ├── Enums/
 ├── Dtos/
 ├── Data/
 ├── Repositories/
 ├── Services/
 ├── Middlewares/
 ├── Program.cs
 ├── appsettings.json
 └── README.md
```

---

# 🔒 Autenticação e Autorização

A API implementa **JWT** para autenticação e roles para autorização.

### ✔ Login

* Endpoint: `/api/auth/login`
* Retorna: `token`, `email`, `role`

### ✔ Roles

* `User`
* `Admin`

### ✔ Exemplo de proteção:

```
[Authorize(Roles = "Admin")]  
```

### ✔ Política STATLESS

Sem sessão. O token é validado em cada requisição.

---

# ⚠️ Tratamento Global de Exceções

A aplicação possui classe `GlobalExceptionHandler` que captura:

* `ValidationException`
* `UnauthorizedAccessException`
* `EntityNotFoundException`
* Qualquer erro inesperado

Retornando sempre JSON padronizado com `ProblemDetails`.

---

# 📘 Documentação Swagger

A API possui documentação completa disponível em:

```
localhost:8080/swagger
```

Inclui:

* Exemplos
* Models de DTOs
* Versionamento (v1 e v2)

---

# 🧩 Versionamento de API

A API suporta múltiplas versões:

```
/api/v1/... 
/api/v2/... 
```

Atualmente ambas operam com os mesmos recursos.

---

# 📌 Funcionalidades da API

Cada módulo possui endpoints completos (GET/POST/PUT/DELETE).

## 🧍 Perfil do Usuário

* GET `/api/v{v}/profile`
* PUT `/api/v{v}/profile`

## 💧 Água

* GET `/api/v{v}/water/today`
* GET `/api/v{v}/water/week`
* POST `/api/v{v}/water`
* PUT `/api/v{v}/water/{id}`
* DELETE `/api/v{v}/water/{id}`

## ☀ Sol

* GET `/api/v{v}/sunlight/today`
* GET `/api/v{v}/sunlight/week`
* POST `/api/v{v}/sunlight`
* PUT `/api/v{v}/sunlight/{id}`
* DELETE `/api/v{v}/sunlight/{id}`

## 🧘 Meditação

* GET `/api/v{v}/meditation/today`
* GET `/api/v{v}/meditation/week`
* POST `/api/v{v}/meditation`
* PUT `/api/v{v}/meditation/{id}`
* DELETE `/api/v{v}/meditation/{id}`

## 😴 Sono

* GET `/api/v{v}/sleep/today`
* GET `/api/v{v}/sleep/week`
* POST `/api/v{v}/sleep`
* PUT `/api/v{v}/sleep/{id}`
* DELETE `/api/v{v}/sleep/{id}`

## 🏃 Atividade Física

* GET `/api/v{v}/activity/today`
* GET `/api/v{v}/activity/week`
* POST `/api/v{v}/activity`
* PUT `/api/v{v}/activity/{id}`
* DELETE `/api/v{v}/activity/{id}`

## 📋 Tarefas

* GET `/api/v{v}/tasks/today`
* GET `/api/v{v}/tasks`
* POST `/api/v{v}/tasks`
* PUT `/api/v{v}/tasks/{id}`
* DELETE `/api/v{v}/tasks/{id}`

---

# 📊 Metas Automáticas

* **Água:** `peso × 35 ml`
* **Sol:** 10 minutos
* **Meditação:** 5 minutos

---

# ▶ Como Executar o Projeto

## 1. Instalar o .NET 8

[https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download)

## 2. Clonar o Repositório

```
git clone <url-do-repositorio>
cd InnerHealth.Api
```

## 3. Rodar a API

```
dotnet run
```

A aplicação estará disponível em:
`http://localhost:5000`

Swagger:
`http://localhost:5000/swagger`

O banco **InnerHealth.db** será criado automaticamente.

---

# 📦 Deploy (Opcional)

O projeto pode ser facilmente publicado via:

* Docker
* Azure App Service
* IIS (Windows)
* Linux + Nginx

---

# 📚 Extensões Futuras

* Dashboard completo com gráficos
* Aplicativo mobile (React Native)
* IA para recomendações de saúde
* Histórico e relatórios avançados

---

# 🏁 Conclusão

A **InnerHealth API** atende integralmente aos requisitos propostos para o projeto de **SOA & WebServices**, oferecendo uma solução completa, modular, extensível e documentada — preparada para integração com front-end, mobile ou serviços externos.

---

© 2025 – InnerHealth API — FIAP Global Solut
