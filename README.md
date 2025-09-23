# rest_rating - Scaffold Hexagonal (exemplo)

Este é um scaffold mínimo de uma API ASP.NET Core com foco em estrutura para o domínio de reviews, restaurantes e usuários.
O propósito é servir como ponto de partida — não uma solução pronta para produção.

## Como rodar (localmente)

Pré-requisitos:
- .NET 8 SDK (ou ajuste o TargetFramework no csproj para sua versão instalada)

No terminal:

```bash
cd rest_rating
dotnet restore
dotnet run
```

A API usará um banco em memória (EF Core InMemory) para facilitar testes locais.

Endpoints principais:
- POST /api/auth/register
- POST /api/auth/login
- GET /api/users/{id}
- POST /api/restaurants
- GET /api/restaurants
- POST /api/restaurants/{restaurantId}/reviews
- GET /api/restaurants/{restaurantId}/reviews/pending
- POST /api/restaurants/{restaurantId}/reviews/{reviewId}/approve

Swagger estará disponível em `/swagger` em ambiente de desenvolvimento.

Observações:
- Autenticação e segurança estão simplificadas (sem JWT implementado).
- Regras de negócio básicas (apenas reviews >=4 aceitos) já estão demonstradas.
