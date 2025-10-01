# 🏦 Steam Currency API


<div align="center">
  
 ### 💹 A modern REST API for currency exchange rates with Steam integration

<div align="center">

![MIT License](https://img.shields.io/badge/License-MIT-yellow.svg)
![.NET](https://img.shields.io/badge/.NET-8.0-blue)
![Docker](https://img.shields.io/badge/Docker-Ready-blue)
![MongoDB](https://img.shields.io/badge/MongoDB-Database-green)

</div>
  
[🌐 Live Frontend](https://steam.nikgob.com) • [📡 API Endpoint](https://steam-api.nikgob.com) • [📖 API Docs](https://steam-api.nikgob.com/swagger)
</div>


<div align="center">

**Язык / Language:** 
[🇷🇺 Русский](README.ru.md) | [🇬🇧 English](README.md)

</div>



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

```
# Clone the repository
git clone https://github.com/NikGob/Steam-Currency.git
cd Steam-Currency

# Build and run with Docker
docker build -t steam-currency-api .
docker run -p 8080:8080 -p 8081:8081 steam-currency-api
```

🎉 API will be available at: http://localhost:8080

### Running Locally

```
# Clone the repository
git clone https://github.com/NikGob/Steam-Currency.git
cd Steam-Currency

# Restore packages
dotnet restore

# Run the application
cd src/SteamCurrencyAPI
dotnet run
```

🎉 API will be available at: https://localhost:5001 or http://localhost:5000

---

## 🗄️ Database Setup

⚠️ **Important**: Before the first launch, you must manually add currency objects to your MongoDB collection — this is required for the service to start fetching exchange rates.

You can add as many currencies as you need — just create a separate object for each one.

### Currency Object Structure
```
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
```
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

---

## 📡 API Endpoints

### Base URL
- **Production**: https://steam-api.nikgob.com
- **Local**: http://localhost:8080 (Docker) or https://localhost:5001 (local)

### Available Endpoints

#### 📊 Get Latest Exchange Rate

```
GET /api/currency/latest-rate?currencyCode=RUB&currencyBaseCode=KZT
```

**Parameters:**
- currencyCode (string): Target currency code
- currencyBaseCode (string): Base currency code

**Example Response:**
```
{
  "currencyCode": "RUB",
  "currencyBaseCode": "KZT",
  "currencyInfo": {
    "dateAtUtc": "2024-01-15",
    "currencyPrice": 5.23
  }
}
```

#### 📈 Get Historical Rates

```
GET /api/currency/rates?currencyCode=RUB&currencyBaseCode=KZT&startDate=2023-01-01&endDate=2023-01-31
```

**Parameters:**
- currencyCode (string): Target currency code
- currencyBaseCode (string): Base currency code
- startDate (string): Start date (YYYY-MM-DD format)
- endDate (string): End date (YYYY-MM-DD format)

#### 🔍 Get Available Currency Codes

```
GET /api/currency/codes
```

**Example Response:**
```
[
  "USD",
  "EUR",
  "RUB",
  "KZT",
  "GBP"
]
```

### 📖 Interactive API Documentation

Visit /swagger endpoint for interactive API documentation:
- **Production**: https://steam-api.nikgob.com/swagger
- **Local**: http://localhost:8080/swagger

---

## 🔧 Configuration

### Environment Variables

```
# Database Configuration
MONGO_CONNECTION_STRING="mongodb://localhost:27017"
MONGO_DATABASE_NAME="SteamCurrencyDB"

# Application Settings
ASPNETCORE_ENVIRONMENT="Development"
ASPNETCORE_URLS="http://+:8080;https://+:8081"
```

### appsettings.json Example

> **Note**: This example configuration is safe to copy as it contains no private tokens or sensitive data.

```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "MongoDB": {
    "ConnectionString": "mongodb/connection/string",
    "DatabaseName": "SteamCurrency",
    "CollectionName": "Сurrencies"
  },
  "SteamAPI": {
    "URL": "https://steamcommunity.com/market/priceoverview/?appid={appId}&market_hash_name={product}&currency={currencyId}"
  }
}
```

---

## 📈 Architecture

### Project Structure

```
Steam-Currency/
├── src/
│   ├── SteamCurrencyAPI/           # Main API project
│   │   ├── Controllers/            # API controllers
│   │   ├── DataWrapper/           # Data access layer
│   │   ├── Interfaces/            # Service interfaces
│   │   ├── Jobs/                  # Background jobs
│   │   ├── Models/                # Data models
│   │   ├── Services/              # Business logic
│   │   ├── Program.cs             # Application entry point
│   │   └── Startup.cs             # Service configuration
│   └── SteamCurrencyAPI.Tests/    # Unit tests
├── Dockerfile                      # Docker configuration
├── LICENSE                        # MIT license
└── README.md                      # This file
```

### Architecture Patterns
- **Clean Architecture**: Separation of concerns with clear layers
- **Dependency Injection**: Built-in DI container
- **Repository Pattern**: Data access abstraction
- **Service Layer**: Business logic encapsulation
- **Background Jobs**: Scheduled tasks with Quartz.NET

---

## 🤝 Contributing

We welcome contributions! Here's how you can help:

### 🔄 How to Contribute

1. Fork the repository
2. Create a feature branch: `git checkout -b feature/amazing-feature`
3. Commit your changes: `git commit -m 'Add amazing feature'`
4. Push to the branch: `git push origin feature/amazing-feature`
5. Open a Pull Request

### 📋 Contribution Guidelines

- Follow C# coding standards
- Add unit tests for new features
- Update documentation as needed
- Ensure Docker builds successfully
- Write clear commit messages

### 🐛 Reporting Issues

Found a bug? Please create an issue with:
- Detailed description
- Steps to reproduce
- Expected vs actual behavior
- Environment details

---

## 📊 Live Deployment

### 🌐 Production URLs

- **Frontend Application**: https://steam.nikgob.com
- **API Endpoint**: https://steam-api.nikgob.com
- **API Documentation**: https://steam-api.nikgob.com/swagger

### 🚀 Deployment Features

- **High Availability**: Load-balanced deployment
- **SSL/HTTPS**: Secure connections
- **MongoDB Atlas**: Cloud database
- **Docker**: Containerized deployment
- **Monitoring**: Health checks and logging

---

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

### 📜 MIT License Summary

- ✅ Commercial use allowed
- ✅ Modification allowed
- ✅ Distribution allowed
- ✅ Private use allowed
- ❗ License and copyright notice required

---

## 📞 Contact

### 👨‍💻 Developer

**NikGob** - Project Maintainer

### 🔗 Links

- 🐙 **GitHub**: [@NikGob](https://github.com/NikGob)
- 🌐 **Website**: [nikgob.com](https://nikgob.com)
- 📧 **API Support**: Contact through GitHub issues

### 🚀 Project Links

- 📦 **Repository**: [Steam-Currency](https://github.com/NikGob/Steam-Currency)
- 🌐 **Live Demo**: [steam.nikgob.com](https://steam.nikgob.com)
- 📡 **API**: [steam-api.nikgob.com](https://steam-api.nikgob.com)

---

### 🙏 Thank you for using Steam Currency API!

If this project helped you, please consider giving it a ⭐

![Made with ❤️](https://img.shields.io/badge/Made%20with-❤️-red.svg) ![C#](https://img.shields.io/badge/C%23-239120?style=flat&logo=c-sharp&logoColor=white) ![.NET Core](https://img.shields.io/badge/.NET%20Core-5C2D91?style=flat&logo=.net&logoColor=white)
