# 🥗 Calorie Count API – Nutrition & Meal Tracker

An extensible **ASP.NET Core Web API** for logging foods, daily meals, and calculating nutritional intake.  The solution follows a clean architecture with separate **Core**, **Infrastructure**, and **Services** layers and ships with a SQL Server backup (`CalorieCount.bak`) containing seed data.

---

## ✨ Core Capabilities

| Feature | Description |
|---------|-------------|
| **Food Catalog** | CRUD endpoints for foods (name, macros, calories), bulk Excel import |
| **Daily Meal Logging** | Track meals composed of multiple foods for any date |
| **User Recipes** | Users can save custom recipes with aggregated nutrition |
| **Tunisian Food** | Pre-seeded dataset of traditional Tunisian meals |
| **Authentication** | AccountController with JWT-based login / register |
| **Excel Upload** | `/api/food/upload` parses `.xlsx` (EPPlus) to seed foods |
| **Swagger** | Auto-generated docs at runtime |

---

## 🗂️ Solution Layout

```
Calorie-Count--main
│
├── Calorie countAPI/                    # Visual Studio solution folder
│   ├── Calorie countAPI/               # ASP.NET Core Web API project (controllers, Program.cs)
│   ├── CalorieCount.Core/              # Domain entities, DTOs, AutoMapper profiles
│   ├── CalorieCount.Services/          # Business logic & service implementations
│   └── CalorieCount.infrastructure/    # EF Core DbContext, repositories
│
├── CalorieCount.bak                    # SQL Server database backup file
└── README.md                           # ← you are here
```

---

## 🛠 Prerequisites

- **.NET 6 SDK**
- **SQL Server Express / LocalDB**
- **Visual Studio 2022** (or CLI)

---

## 🚀 Getting Started

1. **Restore the database** (optional – or use migrations):
   - SSMS → *Restore Database* → select `CalorieCount.bak`.
2. **Update** the connection string in `Calorie countAPI/appsettings.json`.
3. **Run** the API:
   ```bash
   dotnet run --project "Calorie countAPI/Calorie countAPI"
   ```
4. Navigate to `https://localhost:5001/swagger` for the interactive API explorer.

---

## 🔌 Selected Endpoints

| Verb | Route | Purpose |
|------|-------|---------|
| GET | `/api/food` | List foods |
| POST | `/api/food/upload` | Bulk import via Excel |
| POST | `/api/food/Add Food` | Add food JSON |
| GET | `/api/dailymeal/{userId}/{date}` | User daily meals |
| POST | `/api/userrecipe` | Save custom recipe |
| POST | `/api/account/login` | Authenticate & obtain JWT |

See full list in Swagger.

---

## 🧰 Tech Stack

- **ASP.NET Core 6** / C#
- **Entity Framework Core** + SQL Server
- **AutoMapper** for DTO mapping
- **EPPlus** for Excel parsing
- **JWT** authentication

---

## 🤝 Contributing

1. Fork → `git checkout -b feature/xyz`
2. Code & commit → `git commit -m "feat: xyz"`
3. Push → PR

---

## 📜 License

MIT (see `LICENSE`).

---

## 👤 Author

**Oussama Souissi** – [GitHub](https://github.com/Oussama-souissi024)
