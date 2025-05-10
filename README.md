# SSProfilingApp

SSProfilingApp is a .NET 8 Web API developed as a case study to group individuals into profiles based on similarity of personal data using matching algorithms. It follows clean architecture and integrates both local and external similarity comparison techniques.

## Key Features

- Similarity scoring using Levenshtein distance and external Jaro-Winkler API
- Profiling based on rules:
  - Identity number match results in direct profile assignment
  - Weighted similarity based on name, birthplace, birthdate, and nationality
- Re-grouping logic that can recalculate all profile assignments
- Supports inserting multiple individuals through the `/individuals` endpoint

## Tech Stack

- .NET 8 Web API
- Entity Framework Core (Code First)
- SQL Server
- Swashbuckle for Swagger UI
- HttpClient for third-party API integration

## Project Structure

SSProfilingApp/
  ├── SSProfilingApp.Domain         - Entity models
  ├── SSProfilingApp.Application    - Interfaces and DTOs
  ├── SSProfilingApp.Infrastructure - DbContext, implementations, similarity logic
  ├── SSProfilingApp.API            - Controllers and configuration
  └── README.md

## Getting Started

1. Clone the repository:

   git clone https://github.com/BYDOC/SSProfilingApp.git
   cd SSProfilingApp

2. Apply database migrations:

   dotnet ef database update --project SSProfilingApp.Infrastructure/SSProfilingApp.Infrastructure.csproj --startup-project SSProfilingApp.API

3. Run the API:

   dotnet run --project SSProfilingApp.API

4. Access Swagger UI:

   http://localhost:<port>/swagger

## API Endpoints

Method | Route | Description
------ | ----- | -----------
POST   | /individuals | Add one or more individuals (as a list)
POST   | /individuals/with-profiles | Group all individuals into profiles
DELETE | /individuals | Delete all individuals and reset profile IDs
POST   | /api/similarity/jarowinkler | Calculate similarity using external API

## Profiling Logic

- If identity numbers match, similarity score is 1.0
- Otherwise, similarity score is calculated:
  - 50% full name similarity (Levenshtein or Jaro-Winkler)
  - 20% birthplace similarity
  - 15% birthdate match within ±2 days
  - 15% nationality match
- If similarity score is 0.85 or higher, individuals are grouped under the same profile

## External API

Jaro-Winkler similarity is calculated using https://api.tilotech.io.  
The base URL can be set in `appsettings.json`.

## Switching Similarity Algorithm

To change from Levenshtein to Jaro-Winkler, update this line in `SimilarityScoreService`:

```csharp
var calc = _calculatorFactory.Get(Application.Enums.SimilarityAlgorithm.Levenshtein);
```

Change it to:

```csharp
var calc = _calculatorFactory.Get(Application.Enums.SimilarityAlgorithm.JaroWinkler);
```

No other changes are needed.
