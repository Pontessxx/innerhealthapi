# InnerHealth API

## Visão Geral

A **InnerHealth API** é um backend RESTful em C#/.NET 8 criado como parte do projeto interdisciplinar *O Futuro do Trabalho*.  
Ela monitora hábitos de bem-estar e produtividade — hidratação, sol, meditação, sono, atividades físicas e tarefas — oferecendo uma base moderna e extensível para projetos acadêmicos ou evoluções futuras.

A proposta é simples: registrar pequenos hábitos diários e gerar uma visão clara da rotina do usuário, ajudando na construção de práticas saudáveis no trabalho e na vida cotidiana.

---

## Funcionalidades

- **Perfil do Usuário**  
  Peso, altura, idade, horas de sono, qualidade do sono — utilizados para metas e recomendações.
- **Hidratação**  
  Registros de ingestão de água (ml). Meta automática: `peso × 35 ml`.
- **Exposição ao Sol**  
  Sessões diárias em minutos. Meta padrão: **10 min**.
- **Meditação**  
  Sessões em minutos. Meta padrão: **5 min**.
- **Sono**  
  Registros de horas dormidas e qualidade diária.
- **Atividade Física**  
  Modalidade + duração.
- **Tarefas**  
  Criação, edição e conclusão de tarefas diárias.
- **Swagger**  
  Documentação automática acessível em `/swagger`.
- **Versionamento de API**  
  Suporte às versões `v1` e `v2` com rotas `/api/v1/...` e `/api/v2/...`.

---

## Arquitetura da Solução

A estrutura segue um padrão simples, organizado e escalável:

```
Cliente → Controllers → Services → Entity Framework Core → SQLite
```

- **Controllers** lidam com as requisições HTTP.  
- **Services** contêm a lógica de negócios.  
- **DbContext** garante persistência.  
- **SQLite** é usado como banco de dados local baseado em arquivo (`InnerHealth.db`).  

O banco é criado automaticamente na primeira execução.

---

## Como Começar

### 1. Pré-requisitos
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

*(Nenhuma instalação de SQL Server é necessária.)*

---

### 2. Clonar o Repositório

```bash
git clone <url-do-repositorio>
cd InnerHealth-backend/InnerHealth.Api
```

---

### 3. Executar a API

```bash
dotnet run
```

A API inicia em:

- **http://localhost:5000**  
- Swagger em **http://localhost:5000/swagger**

O arquivo **InnerHealth.db** será criado automaticamente.

---

## Versionamento e Rotas

A API usa versionamento direto na URL.  
Exemplos:

```
/api/v1/profile
/api/v2/water/today
```

As versões v1 e v2 atualmente possuem os mesmos endpoints.

---

## Endpoints Principais

`{v}` = versão (1 ou 2)

### Perfil
| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/api/v{v}/profile` | Obtém o perfil |
| PUT | `/api/v{v}/profile` | Atualiza o perfil |

### Água
| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/api/v{v}/water/today` | Dados de hoje |
| GET | `/api/v{v}/water/week` | Totais semanais |
| POST | `/api/v{v}/water` | Registra ingestão |
| PUT | `/api/v{v}/water/{id}` | Edita ingestão |
| DELETE | `/api/v{v}/water/{id}` | Remove ingestão |

### Sol, Meditação, Sono, Atividade Física
Padrão igual ao módulo de água:

- `/today`
- `/week`
- POST (criar)
- PUT (editar)
- DELETE (remover)

### Tarefas
| Método | Rota |
|--------|-------|
| GET | `/api/v{v}/tasks/today` |
| GET | `/api/v{v}/tasks` |
| POST | `/api/v{v}/tasks` |
| PUT | `/api/v{v}/tasks/{id}` |
| DELETE | `/api/v{v}/tasks/{id}` |

---

## Metas Diárias

- **Água:** `peso × 35 ml`  
- **Sol:** 10 minutos  
- **Meditação:** 5 minutos  

As metas são sugeridas e podem ser ultrapassadas ou ajustadas pelos registros do usuário.

---

## Controle Diário

- Cada registro usa `DateOnly`  
- Dados semanais sempre usam **segunda–domingo**  
- Dias sem dados retornam **zero ou nulo**, facilitando gráficos e dashboards.

---

## Autenticação

A API **não possui autenticação** no momento, pois o foco é demonstrar funcionalidades e simplicidade.

Para o futuro:

- JWT  
- múltiplos usuários  
- integração com aplicativos móveis  

---

## Extensões Futuras

- Importação de dados via dispositivos (teclado/mouse, estresse)  
- Relatórios com IA  
- App mobile + notificações  
- Versionamento avançado na v2  
- Deploy em nuvem (Azure, AWS)

---

Sinta-se livre para adaptar, expandir e contribuir com o projeto!