Integrantes do projeto:

Aguinel Junior Bento da Silva - RM564857
Felipe da Silva - RM563485
Henrique Gonçalves - RM562086
Leonardo Saavedra - RM562228
Vitor Mendes - RM565376

# LifePet API

**Fluxo contínuo de cuidados para pets**

API REST desenvolvida em ASP.NET Core para apoiar o controle de saúde e rotina de animais de estimação. O sistema centraliza informações de tutores, pets, vacinas, consultas, medicamentos e histórico clínico, com rotas adicionais para consultas de alertas (vacinas em atraso, medicamentos ativos, consultas futuras).

Projeto da disciplina **Advanced Business Development with .NET** (FIAP) — Sprint 1 e 2.

---

## Tecnologias

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- Oracle Database
- Swagger (OpenAPI)

---

## Modelo de dados

| Entidade    | Descrição                                      |
|-------------|------------------------------------------------|
| Tutor       | Responsável legal pelo animal                  |
| Pet         | Animal de estimação vinculado a um tutor       |
| Vacina      | Registro de vacinação e próxima dose           |
| Consulta    | Agendamento e atendimento veterinário          |
| Medicamento | Tratamento medicamentoso em curso              |
| Histórico   | Registros clínicos (exames, observações, etc.) |

O pet depende de um tutor; vacinas, consultas, medicamentos e históricos dependem de um pet.

---

## Pré-requisitos

- [.NET SDK 10](https://dotnet.microsoft.com/download)
- Acesso a uma instância Oracle (ambiente FIAP ou Oracle XE local)
- [SQL Developer](https://www.oracle.com/database/sqldeveloper/) ou ferramenta equivalente (recomendado)
- [Oracle Instant Client](https://www.oracle.com/database/technologies/instant-client.html), se exigido pelo ambiente

---

## Configuração do banco de dados
### Conexão Oracle

Informe a connection string em `appsettings.json` e `appsettings.Development.json` (mantenha os dois arquivos alinhados):

```json
"OracleConnection": "User Id=RMXXXXXX;Password=080499;Data Source=oracle.fiap.com.br:1521/ORCL"
```

Substitua usuário, senha, host, porta e service name conforme o ambiente disponibilizado. **Não versione senhas reais no repositório.**

Exemplo de configuração na FIAP:

| Parâmetro     | Valor típico              |
|---------------|---------------------------|
| Hostname      | `oracle.fiap.com.br`      |
| Porta         | `1521`                    |
| Service name  | `ORCL`                    |
| Usuário       | RMXXXXXX |

Valide a conexão no SQL Developer antes de executar a API. O teste de conexão deve retornar *Success*.

### Migrations (estrutura das tabelas)

Na raiz do projeto:

```bash
dotnet ef database update
```

Esse comando cria as tabelas `TUTORES`, `PETS`, `VACINAS`, `CONSULTAS`, `MEDICAMENTOS` e `HISTORICOS` no schema do usuário conectado.

### Dados iniciais (opcional)

Para popular o banco com registros de exemplo, execute o script `Scripts/02-dados-teste.sql` no SQL Developer. Também é possível inserir dados pela interface Swagger, conforme a seção [Exemplos de requisição](#exemplos-de-requisição).

O script `Scripts/01-create-schema.sql` destina-se apenas a instalações locais com usuário dedicado (`LIFEPET`); no ambiente FIAP o usuário do aluno já é provisionado.

---

## Execução da API

```bash
dotnet restore
dotnet run
```

Em ambiente de desenvolvimento, a documentação interativa fica disponível em:

**http://localhost:5219/swagger**

A porta pode variar conforme `Properties/launchSettings.json`.

---

## Documentação da API

A especificação OpenAPI é exposta via Swagger. Todos os endpoints podem ser testados diretamente na interface (*Try it out*), o que facilita a validação dos códigos de resposta HTTP exigidos na sprint.

### Códigos de resposta utilizados

| Código | Uso habitual                          |
|--------|---------------------------------------|
| 200    | Consulta realizada com sucesso        |
| 201    | Recurso criado                        |
| 204    | Atualização ou exclusão concluída     |
| 400    | Dados inválidos ou regra de negócio   |
| 404    | Recurso não encontrado                |

---

## Endpoints

As rotas seguem o padrão REST: recursos no plural, identificadores na URL e verbos HTTP semânticos.

### Tutores — `/api/tutores`

| Método | Rota                 | Descrição                    |
|--------|----------------------|------------------------------|
| GET    | `/`                  | Lista todos os tutores       |
| GET    | `/{id}`              | Obtém tutor por identificador |
| GET    | `/email/{email}`     | Busca tutor por e-mail       |
| GET    | `/{tutorId}/pets`    | Lista pets de um tutor       |
| POST   | `/`                  | Cadastra um tutor            |
| PUT    | `/{id}`              | Atualiza um tutor            |
| DELETE | `/{id}`              | Remove um tutor              |

### Pets — `/api/pets`

| Método | Rota                  | Descrição                    |
|--------|-----------------------|------------------------------|
| GET    | `/`                   | Lista todos os pets          |
| GET    | `/{id}`               | Obtém pet por identificador  |
| GET    | `/especie/{especie}`  | Filtra por espécie           |
| GET    | `/tutor/{tutorId}`    | Lista pets de um tutor       |
| POST   | `/`                   | Cadastra um pet              |
| PUT    | `/{id}`               | Atualiza um pet              |
| DELETE | `/{id}`               | Remove um pet                |

### Vacinas — `/api/vacinas`

| Método | Rota             | Descrição                         |
|--------|------------------|-----------------------------------|
| GET    | `/`              | Lista todas as vacinas            |
| GET    | `/{id}`          | Obtém vacina por identificador    |
| GET    | `/pet/{petId}`   | Lista vacinas de um pet           |
| GET    | `/atrasadas`     | Vacinas com dose em atraso        |
| POST   | `/`              | Registra uma vacina               |
| PUT    | `/{id}`          | Atualiza um registro              |
| DELETE | `/{id}`          | Remove um registro                |

### Consultas — `/api/consultas`

| Método | Rota             | Descrição                         |
|--------|------------------|-----------------------------------|
| GET    | `/`              | Lista todas as consultas          |
| GET    | `/{id}`          | Obtém consulta por identificador  |
| GET    | `/pet/{petId}`   | Lista consultas de um pet         |
| GET    | `/futuras`       | Consultas com data futura         |
| POST   | `/`              | Agenda uma consulta               |
| PUT    | `/{id}`          | Atualiza um agendamento           |
| DELETE | `/{id}`          | Remove um agendamento             |

### Medicamentos — `/api/medicamentos`

| Método | Rota             | Descrição                         |
|--------|------------------|-----------------------------------|
| GET    | `/`              | Lista todos os medicamentos       |
| GET    | `/{id}`          | Obtém medicamento por identificador |
| GET    | `/pet/{petId}`   | Lista medicamentos de um pet      |
| GET    | `/ativos`        | Tratamentos em andamento          |
| POST   | `/`              | Registra um medicamento           |
| PUT    | `/{id}`          | Atualiza um registro              |
| DELETE | `/{id}`          | Remove um registro                |

### Históricos — `/api/historicos`

| Método | Rota             | Descrição                         |
|--------|------------------|-----------------------------------|
| GET    | `/`              | Lista todo o histórico            |
| GET    | `/{id}`          | Obtém registro por identificador  |
| GET    | `/pet/{petId}`   | Histórico de um pet               |
| GET    | `/tipo/{tipo}`   | Filtra por tipo de registro       |
| POST   | `/`              | Inclui um registro                |
| PUT    | `/{id}`          | Atualiza um registro              |
| DELETE | `/{id}`          | Remove um registro                |

---

## Exemplos de requisição

Cadastre primeiro um tutor; em seguida um pet referenciando o `tutorId` retornado.

**Tutor** — `POST /api/tutores`

```json
{
  "nome": "Maria Silva",
  "email": "maria@email.com",
  "telefone": "11999999999"
}
```

**Pet** — `POST /api/pets`

```json
{
  "nome": "Thor",
  "especie": "Cão",
  "raca": "Labrador",
  "dataNascimento": "2020-05-10T00:00:00",
  "tutorId": 1
}
```

Demais recursos seguem a mesma lógica, utilizando o `petId` do animal cadastrado.

---

## Estrutura do repositório

```
LifePetApi/
├── Controllers/       Endpoints REST por entidade
├── Data/              DbContext e configuração EF Core
├── Models/            Classes de domínio
├── Migrations/        Evolução do schema no Oracle
├── Scripts/           Scripts SQL auxiliares
├── Program.cs         Configuração da aplicação e Swagger
└── appsettings.json   Connection string e parâmetros
```

---

## Migrations

Após alterar as entidades em `Models/` ou o mapeamento em `Data/LifePetDbContext.cs`:

```bash
dotnet ef migrations add DescricaoDaAlteracao
dotnet ef database update
```

---

## Autoria

Projeto acadêmico — FIAP, Advanced Business Development with .NET.

#   S P R I N T 1 - 2 - d o t . n e t 
 
 #   S P R I N T 1 - 2 - d o t . n e t 
 
 