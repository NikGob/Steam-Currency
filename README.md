---
## 📋 Table of Contents
- [✨ Features](#-features)
- [🛠 Tech Stack](#-tech-stack)
- [🚀 Quick Start](#-quick-start)
  - [Prerequisites](#prerequisites)
  - [Running with Docker](#running-with-docker)
  - [Running Locally](#running-locally)
- [🗄️ Database Setup](#️-database-setup)
- [📡 API Endpoints](#-api-endpoints)
- [🔧 Configuration](#-configuration)
- [📈 Architecture](#-architecture)
- [🤝 Contributing](#-contributing)
- [📄 License](#-license)
- [📞 Contact](#-contact)

---

## ✨ Features

🔥 **Core Functionality**
- 📊 Real-time currency exchange rates
- 📈 Historical rate tracking and analysis
- 🔄 Automatic data updates with background jobs
- 🌍 Support for multiple currency pairs
- 📱 RESTful API with comprehensive documentation

⚡ **Technical Features**
- 🏗️ Clean Architecture with SOLID principles
- 🐳 Docker containerization ready
- 📊 MongoDB integration for data persistence
- 🔄 Quartz.NET for scheduled jobs
- 📝 Swagger/OpenAPI documentation
- ⚡ Asynchronous operations

---

## 🛠 Tech Stack

### Backend
- **Framework**: ASP.NET Core 8.0
- **Database**: MongoDB
- **Job Scheduling**: Quartz.NET
- **Documentation**: Swagger/OpenAPI
- **Containerization**: Docker
- **Language**: C# (.NET 8.0)

### Key Dependencies
- Microsoft.Extensions.Hosting - Hosting infrastructure
- MongoDB.Driver - MongoDB integration
- Newtonsoft.Json - JSON serialization
- Quartz.AspNetCore - Background job scheduling
- Swashbuckle.AspNetCore - API documentation

---

## 🚀 Quick Start

### Prerequisites
- **Option 1** (Docker): Docker & Docker Compose
- **Option 2** (Local): .NET 8.0 SDK + MongoDB instance

### Running with Docker
```bash
# Clone the repository
git clone https://github.com/NikGob/Steam-Currency.git
cd Steam-Currency

# Build and run with Docker
docker build -t steam-currency-api .
docker run -p 8080:8080 -p 8081:8081 steam-currency-api
```

🎉 API will be available at: [http://localhost:8080](http://localhost:8080)

### Running Locally
```bash
# Clone the repository
git clone https://github.com/NikGob/Steam-Currency.git
cd Steam-Currency

# Restore packages
dotnet restore

# Run the application
dotnet run
```

---

## 🗄️ Database Setup

⚠️ **Important**: Before the first launch, you must manually add currency objects to your MongoDB collection — this is required for the service to start fetching exchange rates.

You can add as many currencies as you need — just create a separate object for each one.

### Currency Object Structure
```json
{
  "CurrencyCode": "USD",
  "CurrencyDatas": [],
  "CreatedAtUTC": "2025-10-01T00:00:00Z"
}
```

**Field Descriptions:**
- `CurrencyCode` — Currency code (e.g., USD, EUR, RUB, etc.)
- `CurrencyDatas` — Array for storing historical rates (will be filled automatically)
- `CreatedAtUTC` — Date when the currency was added (ISO 8601)

### Sample Script for Mongo Shell
```javascript
use SteamCurrency

db.Сurrencies.insertMany([
  {
    "CurrencyCode": "USD",
    "CurrencyDatas": [],
    "CreatedAtUTC": new Date()
  },
  {
    "CurrencyCode": "EUR",
    "CurrencyDatas": [],
    "CreatedAtUTC": new Date()
  },
  {
    "CurrencyCode": "RUB",
    "CurrencyDatas": [],
    "CreatedAtUTC": new Date()
  }
])
```

👉 **After this step**, exchange rates will start updating automatically via background jobs. You don't need to modify the `CurrencyDatas` array manually.
