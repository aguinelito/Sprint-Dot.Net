# Agrosphere

API REST desenvolvida em ASP.NET Core para monitoramento inteligente de fazendas. O projeto centraliza usuarios, fazendas, sensores IoT, leituras, alertas climaticos, previsoes e historico de eventos, usando Entity Framework Core com Oracle Database.

Projeto academico da disciplina Advanced Business Development with .NET (FIAP).

## Repositorio

Codigo-fonte disponivel em:

[https://github.com/aguinelito/Sprint-Dot.Net.git](https://github.com/aguinelito/ProjetoAgrosphere.net)

## Integrantes

- Aguinel Junior Bento da Silva - RM564857
- Felipe da Silva - RM563485
- Henrique Goncalves - RM562086
- Leonardo Saavedra - RM562228
- Vitor Mendes - RM565376

## Tecnologias

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- Oracle Database
- Swagger / OpenAPI

## Modelo de dados

| Entidade | Descricao | Tabela |
| --- | --- | --- |
| Usuario | Proprietario ou gerente das fazendas monitoradas | USUARIOS_MONITORAMENTO |
| Fazenda | Propriedade rural vinculada a um usuario | FAZENDAS_MONITORAMENTO |
| Sensor | Dispositivo IoT instalado em uma fazenda | SENSORES_MONITORAMENTO |
| Leitura | Medicao capturada por um sensor | LEITURAS_SENSORES |
| AlertaClimatico | Alerta gerado a partir de condicoes criticas | ALERTAS_CLIMATICOS_SENSORES |
| Previsao | Previsao e recomendacao para apoio a decisao | PREVISOES_SENSORES |
| HistoricoLeitura | Registro historico de leituras, manutencoes e eventos | HISTORICO_LEITURAS_SENSORES |

Relacionamentos principais:

```text
Usuario 1:N Fazenda
Fazenda 1:N Sensor
Sensor 1:N Leitura
Sensor 1:N AlertaClimatico
Sensor 1:N Previsao
Sensor 1:N HistoricoLeitura
```

## Pre-requisitos

- .NET SDK 10
- Acesso a uma instancia Oracle
- Entity Framework Core Tools
- SQL Developer ou ferramenta equivalente

Instalacao do EF Core Tools, caso ainda nao esteja disponivel:

```bash
dotnet tool install --global dotnet-ef
```

## Configuracao

Configure a connection string em `appsettings.json` e, se usar ambiente local de desenvolvimento, tambem em `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "OracleConnection": "User Id=RMXXXXXX;Password=SUA_SENHA;Data Source=oracle.fiap.com.br:1521/ORCL"
  }
}
```

Nao versione senhas reais no repositorio. Para entrega academica, mantenha placeholders como `RMXXXXXX` e `SUA_SENHA`.

## Como executar

Na raiz do projeto:

```bash
dotnet restore
dotnet ef database update
dotnet run
```

Em desenvolvimento, a documentacao interativa fica disponivel em:

```text
http://localhost:5219/swagger
```

A porta pode variar conforme `Properties/launchSettings.json`.

## Endpoints

### Usuarios - `/api/usuarios`

| Metodo | Rota | Descricao |
| --- | --- | --- |
| GET | `/` | Lista usuarios |
| GET | `/{id}` | Busca usuario por id |
| POST | `/` | Cria usuario |
| PUT | `/{id}` | Atualiza usuario |
| DELETE | `/{id}` | Remove usuario |

### Fazendas - `/api/fazendas`

| Metodo | Rota | Descricao |
| --- | --- | --- |
| GET | `/` | Lista fazendas |
| GET | `/{id}` | Busca fazenda por id |
| GET | `/usuario/{usuarioId}` | Lista fazendas de um usuario |
| POST | `/` | Cria fazenda |
| PUT | `/{id}` | Atualiza fazenda |
| DELETE | `/{id}` | Remove fazenda |

### Sensores - `/api/sensores`

| Metodo | Rota | Descricao |
| --- | --- | --- |
| GET | `/` | Lista sensores |
| GET | `/{id}` | Busca sensor por id |
| GET | `/fazenda/{fazendaId}` | Lista sensores de uma fazenda |
| POST | `/` | Cria sensor |
| PUT | `/{id}` | Atualiza sensor |
| DELETE | `/{id}` | Remove sensor |

### Leituras - `/api/leituras`

| Metodo | Rota | Descricao |
| --- | --- | --- |
| GET | `/` | Lista leituras |
| GET | `/{id}` | Busca leitura por id |
| GET | `/sensor/{sensorId}` | Lista leituras de um sensor |
| GET | `/sensor/{sensorId}/ultimas/{quantidade}` | Lista as ultimas leituras de um sensor |
| POST | `/` | Registra leitura |
| PUT | `/{id}` | Atualiza leitura |
| DELETE | `/{id}` | Remove leitura |

### Alertas climaticos - `/api/alertasClimaticos`

| Metodo | Rota | Descricao |
| --- | --- | --- |
| GET | `/` | Lista alertas ativos |
| GET | `/{id}` | Busca alerta por id |
| GET | `/sensor/{sensorId}` | Lista alertas de um sensor |
| GET | `/sensor/{sensorId}/ativos` | Lista alertas ativos de um sensor |
| POST | `/` | Cria alerta |
| PUT | `/{id}` | Atualiza alerta |
| PUT | `/{id}/resolver` | Marca alerta como resolvido |
| DELETE | `/{id}` | Remove alerta |

### Previsoes - `/api/previsoes`

| Metodo | Rota | Descricao |
| --- | --- | --- |
| GET | `/` | Lista previsoes |
| GET | `/{id}` | Busca previsao por id |
| GET | `/sensor/{sensorId}` | Lista previsoes de um sensor |
| GET | `/sensor/{sensorId}/vigentes` | Lista previsoes dentro da vigencia |
| POST | `/` | Cria previsao |
| PUT | `/{id}` | Atualiza previsao |
| DELETE | `/{id}` | Remove previsao |

## Exemplos de requisicao

### Criar usuario

```http
POST /api/usuarios
Content-Type: application/json

{
  "nome": "Joao Silva",
  "email": "joao@fazenda.com",
  "telefone": "11999999999"
}
```

### Criar fazenda

```http
POST /api/fazendas
Content-Type: application/json

{
  "nome": "Fazenda Esperanca",
  "localizacao": "Goias - Brasil",
  "descricao": "Producao de graos e soja",
  "usuarioId": 1
}
```

### Criar sensor

```http
POST /api/sensores
Content-Type: application/json

{
  "nome": "Sensor Temperatura - Campo Norte",
  "tipo": "TEMPERATURA",
  "localizacao": "Campo Norte",
  "dataInstalacao": "2026-06-07T00:00:00",
  "fazendaId": 1
}
```

### Registrar leitura

```http
POST /api/leituras
Content-Type: application/json

{
  "dataHora": "2026-06-07T14:30:00",
  "valor": 28.5,
  "unidade": "C",
  "sensorId": 1
}
```

## Estrutura do projeto

```text
Agrosphere/
|-- Controllers/       Endpoints REST
|-- Data/              DbContext e mapeamento EF Core
|-- Models/            Entidades de dominio
|-- Migrations/        Migrations do banco Oracle
|-- Scripts/           Scripts SQL auxiliares
|-- wwwroot/           Customizacoes estaticas do Swagger
|-- Program.cs         Configuracao da aplicacao
|-- Agrosphere.csproj  Projeto .NET
`-- README.md          Documentacao do projeto
```

## Migrations

Para criar uma nova migration apos alterar entidades ou mapeamentos:

```bash
dotnet ef migrations add NomeDaMigration
dotnet ef database update
```

## Licenca

Projeto desenvolvido para fins educacionais.
