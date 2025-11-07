# FakeStoreAPI — Web API em .NET 8

## Visão geral
Este repositório contém uma API REST construída com ASP.NET Core (.NET 8). O projeto já vem estruturado com camadas simples (Controllers → Services → Repositories), injeção de dependência, logging com Serilog, configuração via `appsettings.json` e Swagger opcional para documentação e testes.

O propósito é consumir a API fakestoreapi.com tratando os dados obtidos.

## Tecnologias e Bibliotecas Essenciais
- **.NET Core 8.0**: Plataforma de desenvolvimento principal.

- **HttpClient**: Biblioteca que faz as requisições e os tratamentos Http.

- **Swashbuckle.AspNetCore (Swagger/OpenAPI)**: Biblioteca para documentação usando o Swagger.

- **AutoMapper**: Biblioteca usada para mapear os objetos consumidos em objetos criados pela API.

- **Microsoft.Extensions.Configuration**: Responsável por fazer leitura e escrita de arquivos de configuração.

- **Serilog**: Responsável por fazer a escrita em arquivos de Log.

## Estrutura do Projeto
O sistema é dividido em um projeto:
- `FakeStoreAPI.Host`:
    - **Responsibilidade**: É o ponto de entrada executável (`.exe`). Ele monta e executa a aplicação.
    - **Contém**: Todas as pastas das classes usadas pela API além do Program.cs com a configuração essencial para iniciar a aplicação.
 
## Endpoints

### Produtos
- `GET /api/v1/Product`
    - Retorna todos os produtos da FakeStore API.
    - Resposta 200: Lista de produtos mapeados.
    - Resposta 400: Detalhes do erro em caso de falha, com log registrado.

- `GET /api/v1/Product/{id}`
    - Retorna um produto específico por ID.
    - Resposta 200: Produto mapeado.
    - Resposta 400: Detalhes do erro em caso de falha, com log registrado.

### Carrinhos
- `GET /api/v1/Cart`
    - Retorna todos os carrinhos da FakeStore API.
    - Resposta 200: Lista de carrinhos mapeados.
    - Resposta 400: Detalhes do erro em caso de falha, com log registrado.

- `GET /api/v1/Cart/{id}`
    - Retorna um carrinho específico por ID.
    - Resposta 200: Carrinho mapeado.
    - Resposta 400: Detalhes do erro em caso de falha, com log registrado.

### Usuários
- `GET /api/v1/User`
    - Retorna todos os usuários da FakeStore API.
    - Resposta 200: Lista de usuários mapeados.
    - Resposta 400: Detalhes do erro em caso de falha, com log registrado.

- `GET /api/v1/User/{id}`
    - Retorna um usuário específico por ID.
    - Resposta 200: Usuário mapeado.
    - Resposta 400: Detalhes do erro em caso de falha, com log registrado.

## Configuração

### appsettings.json
Configurações principais da aplicação:

**Seção `Startup`:**
- `UseSwagger` ("true" | "false"): Habilita/desabilita o uso do Swagger.
- `LogDirectory` (string): Pasta para gravação dos logs (relativa ao diretório base da aplicação).

**Seção `Logging`:**
- `LogLevel.Default` (string): Nível de log padrão da aplicação.
- `LogLevel.Microsoft.AspNetCore` (string): Nível de log específico para o framework ASP.NET Core.

**Configurações de integração:**
- `FakeStoreUrl` (string): URL base da API externa FakeStore (https://fakestoreapi.com/).
- `TimeOut` (int): Tempo limite em milissegundos para requisições HTTP aos clientes externos (padrão: 30000ms).
- `AllowedHosts` (string): Hosts permitidos para acessar a aplicação (padrão: "*" permite todos).

### appsettings.Production.json
Configuração específica para ambiente de produção:
- Define o Kestrel para escutar HTTPS na porta 443 (padrão web).
- Usado quando `ASPNETCORE_ENVIRONMENT=Production`.

### Perfis de execução (local)
Definidos em `API.Host/Properties/launchSettings.json`:
- HTTPS: `https://localhost:5001`
- Ambiente padrão: `ASPNETCORE_ENVIRONMENT=Development`

## Arquitetura e Padrões de Projeto
- Hospedagem e pipeline
    - Usa o `WebApplication` (minimal hosting) do ASP.NET Core.
    - Middlewares: HTTPS redirection, Authorization (sem políticas ativas por padrão) e mapeamento de controllers.
    - Swagger/UI habilitado condicionalmente via configuração.

- Injeção de dependência
    - Camadas separadas para facilitar testes e evolução.

- Logging (Serilog)
    - Logs em console e arquivo rolling diário em `logs/system_log_.txt` (diretório configurável).
    - Em falhas na inicialização, um arquivo é escrito em `StartupErrors/` para garantir rastreabilidade mesmo antes do logger.

- Tratamento de erros
    - Exceções em endpoints são capturadas no controller e logadas antes de retornar `400 Bad Request`.

## Uso da API
A API pode ser usada via console ao compilar o código e usar o .exe dentro do terminal, dependendo da configuração do appsettings.json vai abrir ou não o swagger no navegador padrão da máquina, quando usada para desenvolvimento (Debug, IDE) também vai abrir automaticamente o navegador.
