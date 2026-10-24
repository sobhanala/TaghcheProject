# Taaghche Book API with Layered Caching

An ASP.NET Core minimal API that retrieves book data through a chain of data sources: in-memory cache, Redis, and the Taaghche upstream API.

## Architecture

- `Endpoints/BookEndpoints.cs`: HTTP routes.
- `Services/`: book-service interface and implementation.
- `Infrastructure/`: data-source chain and cache options.
- `Domain/Book.cs`: book model.

## Run locally

Requirements: .NET 10 SDK, Redis, and network access to the configured upstream API.

1. Start a local Redis instance, or configure another accessible Redis server.
2. Review `appsettings.json`: `BookApi:BaseUrl` selects the upstream service and `BookCache` controls Redis connection, key prefix, and cache durations.
3. Build and start:

```sh
dotnet restore
dotnet build
dotnet run
```

Use the address printed at startup. Request `GET /books/{id}` with a positive integer ID. Missing books return HTTP 404.

Default cache durations are five minutes in memory and one hour in Redis. Set deployment-specific credentials through environment variables or user secrets.
