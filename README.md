# CarMark

## Executar com Docker

Com Docker Desktop instalado e em execução, na raiz do projeto execute:

```bash
docker compose up --build
```

Abra `http://localhost:8080`. O banco SQLite e as chaves de autenticação ficam no volume Docker `carmark-data`, portanto os usuários e anúncios permanecem após reiniciar os contêineres.

Para encerrar:

```bash
docker compose down
```

Para apagar também os dados locais criados pelo Docker:

```bash
docker compose down -v
```

## Executar sem Docker

```bash
dotnet run --project CarMark/CarMark.csproj
```

O aplicativo cria automaticamente o banco em `CarMark/App_Data/carmark.db`; não há credenciais nem serviço externo a configurar.
