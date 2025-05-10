# SSProfilingApp

SSProfilingApp is a .NET 8 Web API developed as a case study to group individuals into profiles based on similarity of personal data using  matching algorithms. It demonstrates clean architecture principles and integrates both local and external similarity comparison techniques.

## ✨ Key Features

- 🔍 **Similarity Scoring** with Levenshtein distance and external Jaro-Winkler API
- 👥 **Automatic Profiling**:
  - Identity Number match → direct profile assignment
  - Weighted scoring based on name, birthplace, birthdate, nationality
- 🔁 **Re-grouping Logic**:
  - Ability to reset and recalculate all profile assignments
- 📥 **Bulk Insert Support** via `/individuals` endpoint

## 🛠 Tech Stack

- .NET 8 Web API
- Entity Framework Core (Code First)
- SQL Server
- Swashbuckle (Swagger)
- HttpClient for third-party integration

## 📁 Project Structure

```
SSProfilingApp/
│
├── SSProfilingApp.Domain         # Entity models (e.g., IndividualData, DataProfile)
├── SSProfilingApp.Application    # Service contracts, DTOs, Interfaces
├── SSProfilingApp.Infrastructure # EF DbContext, implementations, similarity logic
├── SSProfilingApp.API            # Controllers, request handling, Swagger setup
└── README.md
```

## 🚀 Getting Started

### 1. Clone the repository
```bash
git clone https://github.com/BYDOC/SSProfilingApp.git
cd SSProfilingApp
```

### 2. Apply migrations and update database
```bash
dotnet ef database update --project SSProfilingApp.Infrastructure/SSProfilingApp.Infrastructure.csproj --startup-project SSProfilingApp.API
```

### 3. Run the API
```bash
dotnet run --project SSProfilingApp.API
```

### 4. Access Swagger UI
```
http://localhost:<port>/swagger
```

## 📬 API Endpoints

| Method | Route                           | Description                                 |
|--------|----------------------------------|---------------------------------------------|
| POST   | `/individuals`                  | Add one or more individuals (as a list)     |
| POST   | `/individuals/with-profiles`    | Group all individuals into profiles         |
| DELETE | `/individuals`                  | Delete all individuals and reset profile IDs|
| POST   | `/api/similarity/jarowinkler`   | Calculate similarity via 3rd-party API      |

## 🧠 Profiling Logic

- **IdentityNumber match** → 1.0 score (same profile)
- **Otherwise**, score is calculated using:
  - Full name (50%) via Levenshtein/Jaro-Winkler
  - Birthplace (20%)
  - Birthdate ±2 days (15%)
  - Nationality exact match (15%)
- Individuals are grouped together if similarity score ≥ `0.85`

## 🌐 External API Usage

The Jaro-Winkler similarity is calculated via `https://api.tilotech.io`.  
You can configure the base URL in `appsettings.json`.

