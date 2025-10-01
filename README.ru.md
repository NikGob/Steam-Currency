# 🏦 Steam Currency API

<div align="center">
  
 ### 💹 Современный REST API для курсов валют в реальном времени с интеграцией Steam
 
</div>

<div align="center">

![MIT License](https://img.shields.io/badge/License-MIT-yellow.svg)
![.NET](https://img.shields.io/badge/.NET-8.0-blue)
![Docker](https://img.shields.io/badge/Docker-Ready-blue)
![MongoDB](https://img.shields.io/badge/MongoDB-Database-green)

</div>

<div align="center">
  
[🌐 Фронтенд](https://steam.nikgob.com) • [📡 API](https://steam-api.nikgob.com) • [📖 Документация API](https://steam-api.nikgob.com/swagger)
 
</div>

<div align="center">
  
**Язык / Language:** 
[🇷🇺 Русский](README.ru.md) | [en English](README.md)

</div>




---

## 📋 Содержание

- [✨ Возможности](#-возможности)
- [🛠 Технологический стек](#-технологический-стек)
- [🚀 Быстрый старт](#-быстрый-старт)
  - [Требования](#требования)
  - [Запуск с Docker](#запуск-с-docker)
  - [Локальный запуск](#локальный-запуск)
- [🗄️ Настройка базы данных](#️-настройка-базы-данных)
- [📡 API эндпоинты](#-api-эндпоинты)
- [🔧 Конфигурация](#-конфигурация)
- [📈 Архитектура](#-архитектура)
- [🤝 Вклад в проект](#-вклад-в-проект)
- [📄 Лицензия](#-лицензия)
- [📞 Контакты](#-контакты)

---

## ✨ Возможности

🔥 **Основной функционал**
- 📊 Курсы валют в реальном времени
- 📈 Отслеживание и анализ исторических данных
- 🔄 Автоматическое обновление данных через фоновые задачи
- 🌍 Поддержка множества валютных пар
- 📱 RESTful API с полной документацией

⚡ **Технические особенности**
- 🏗️ Чистая архитектура с принципами SOLID
- 🐳 Готовая контейнеризация Docker
- 📊 Интеграция с MongoDB для хранения данных
- 🔄 Quartz.NET для запланированных задач
- 📝 Документация Swagger/OpenAPI
- ⚡ Асинхронные операции

---

## 🛠 Технологический стек

### Backend
- **Фреймворк**: ASP.NET Core 8.0
- **База данных**: MongoDB
- **Планировщик задач**: Quartz.NET
- **Документация**: Swagger/OpenAPI
- **Контейнеризация**: Docker
- **Язык**: C# (.NET 8.0)

### Основные зависимости
- Microsoft.Extensions.Hosting - инфраструктура хостинга
- MongoDB.Driver - интеграция с MongoDB
- Newtonsoft.Json - сериализация JSON
- Quartz.AspNetCore - фоновые задачи по расписанию
- Swashbuckle.AspNetCore - документация API

---

## 🚀 Быстрый старт

### Требования
- **Вариант 1** (Docker): Docker и Docker Compose
- **Вариант 2** (Локально): .NET 8.0 SDK + экземпляр MongoDB

### Запуск с Docker

```
# Клонируйте репозиторий
git clone https://github.com/NikGob/Steam-Currency.git
cd Steam-Currency

# Соберите и запустите с Docker
docker build -t steam-currency-api .
docker run -p 8080:8080 -p 8081:8081 steam-currency-api
```

🎉 API будет доступен по адресу: http://localhost:8080

### Локальный запуск

```
# Клонируйте репозиторий
git clone https://github.com/NikGob/Steam-Currency.git
cd Steam-Currency

# Восстановите пакеты
dotnet restore

# Запустите приложение
cd src/SteamCurrencyAPI
dotnet run
```

🎉 API будет доступен по адресу: https://localhost:5001 или http://localhost:5000

---

## 🗄️ Настройка базы данных

⚠️ **Важно**: Перед первым запуском необходимо вручную добавить объекты валют в коллекцию MongoDB — это обязательное требование для начала получения курсов.

Вы можете добавить столько валют, сколько нужно — просто создайте отдельный объект для каждой.

### Структура объекта валюты
```
{
  "CurrencyCode": "USD",
  "CurrencyDatas": [],
  "CreatedAtUTC": "2025-10-01T00:00:00Z"
}
```

**Описание полей:**
- `CurrencyCode` — код валюты (например, USD, EUR, RUB и т.д.)
- `CurrencyDatas` — массив для хранения исторических курсов (заполняется автоматически)
- `CreatedAtUTC` — дата добавления валюты (формат ISO 8601)

### Пример скрипта для Mongo Shell
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

👉 **После этого шага** курсы валют начнут обновляться автоматически через фоновые задачи. Вам не нужно изменять массив `CurrencyDatas` вручную.

---

## 📡 API эндпоинты

### Базовый URL
- **Продакшн**: https://steam-api.nikgob.com
- **Локально**: http://localhost:8080 (Docker) или https://localhost:5001 (локально)

### Доступные эндпоинты

#### 📊 Получить последний курс обмена

```
GET /api/currency/latest-rate?currencyCode=RUB&currencyBaseCode=KZT
```

**Параметры:**
- currencyCode (string): код целевой валюты
- currencyBaseCode (string): код базовой валюты

**Пример ответа:**
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

#### 📈 Получить исторические курсы

```
GET /api/currency/rates?currencyCode=RUB&currencyBaseCode=KZT&startDate=2023-01-01&endDate=2023-01-31
```

**Параметры:**
- currencyCode (string): код целевой валюты
- currencyBaseCode (string): код базовой валюты
- startDate (string): дата начала (формат YYYY-MM-DD)
- endDate (string): дата окончания (формат YYYY-MM-DD)

#### 🔍 Получить доступные коды валют

```
GET /api/currency/codes
```

**Пример ответа:**
```
[
  "USD",
  "EUR",
  "RUB",
  "KZT",
  "GBP"
]
```

### 📖 Интерактивная документация API

Посетите эндпоинт /swagger для интерактивной документации API:
- **Продакшн**: https://steam-api.nikgob.com/swagger
- **Локально**: http://localhost:8080/swagger

---

## 🔧 Конфигурация

### Переменные окружения

```
# Конфигурация базы данных
MONGO_CONNECTION_STRING="mongodb://localhost:27017"
MONGO_DATABASE_NAME="SteamCurrencyDB"

# Настройки приложения
ASPNETCORE_ENVIRONMENT="Development"
ASPNETCORE_URLS="http://+:8080;https://+:8081"
```

### Пример appsettings.json

> **Примечание**: Эта конфигурация безопасна для копирования, так как не содержит приватных токенов или чувствительных данных.

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

## 📈 Архитектура

### Структура проекта

```
Steam-Currency/
├── src/
│   ├── SteamCurrencyAPI/           # Основной проект API
│   │   ├── Controllers/            # Контроллеры API
│   │   ├── DataWrapper/           # Слой доступа к данным
│   │   ├── Interfaces/            # Интерфейсы сервисов
│   │   ├── Jobs/                  # Фоновые задачи
│   │   ├── Models/                # Модели данных
│   │   ├── Services/              # Бизнес-логика
│   │   ├── Program.cs             # Точка входа приложения
│   │   └── Startup.cs             # Конфигурация сервисов
│   └── SteamCurrencyAPI.Tests/    # Юнит-тесты
├── Dockerfile                      # Конфигурация Docker
├── LICENSE                        # Лицензия MIT
└── README.md                      # Этот файл
```

### Архитектурные паттерны
- **Чистая архитектура**: Разделение ответственности с четкими слоями
- **Внедрение зависимостей**: Встроенный DI-контейнер
- **Паттерн репозитория**: Абстракция доступа к данным
- **Слой сервисов**: Инкапсуляция бизнес-логики
- **Фоновые задачи**: Запланированные задачи с Quartz.NET

---

## 🤝 Вклад в проект

Мы приветствуем ваш вклад! Вот как вы можете помочь:

### 🔄 Как внести вклад

1. Сделайте форк репозитория
2. Создайте ветку для функции: `git checkout -b feature/amazing-feature`
3. Закоммитьте изменения: `git commit -m 'Add amazing feature'`
4. Отправьте в ветку: `git push origin feature/amazing-feature`
5. Откройте Pull Request

### 📋 Правила участия

- Следуйте стандартам кодирования C#
- Добавляйте юнит-тесты для новых функций
- Обновляйте документацию по мере необходимости
- Убедитесь, что Docker собирается успешно
- Пишите понятные сообщения коммитов

### 🐛 Сообщение об ошибках

Нашли баг? Пожалуйста, создайте issue с:
- Подробным описанием
- Шагами для воспроизведения
- Ожидаемым и фактическим поведением
- Деталями окружения

---

## 📊 Развертывание в продакшн

### 🌐 URL продакшн-версии

- **Фронтенд-приложение**: https://steam.nikgob.com
- **API эндпоинт**: https://steam-api.nikgob.com
- **Документация API**: https://steam-api.nikgob.com/swagger

### 🚀 Возможности развертывания

- **Высокая доступность**: Балансировка нагрузки
- **SSL/HTTPS**: Защищенные соединения
- **MongoDB Atlas**: Облачная база данных
- **Docker**: Контейнеризованное развертывание
- **Мониторинг**: Проверки состояния и логирование

---

## 📄 Лицензия

Этот проект лицензирован под лицензией MIT - см. файл [LICENSE](LICENSE) для подробностей.

### 📜 Краткое описание лицензии MIT

- ✅ Коммерческое использование разрешено
- ✅ Модификация разрешена
- ✅ Распространение разрешено
- ✅ Частное использование разрешено
- ❗ Требуется уведомление о лицензии и авторских правах

---

## 📞 Контакты

### 👨‍💻 Разработчик

**NikGob** - сопровождающий проекта

### 🔗 Ссылки

- 🐙 **GitHub**: [@NikGob](https://github.com/NikGob)
- 🌐 **Веб-сайт**: [nikgob.com](https://nikgob.com)
- 📧 **Поддержка API**: Обращайтесь через GitHub issues

### 🚀 Ссылки на проект

- 📦 **Репозиторий**: [Steam-Currency](https://github.com/NikGob/Steam-Currency)
- 🌐 **Живое демо**: [steam.nikgob.com](https://steam.nikgob.com)
- 📡 **API**: [steam-api.nikgob.com](https://steam-api.nikgob.com)

---

### 🙏 Спасибо, что используете Steam Currency API!

Если этот проект вам помог, пожалуйста, поставьте ⭐

![Made with ❤️](https://img.shields.io/badge/Made%20with-❤️-red.svg) ![C#](https://img.shields.io/badge/C%23-239120?style=flat&logo=c-sharp&logoColor=white) ![.NET Core](https://img.shields.io/badge/.NET%20Core-5C2D91?style=flat&logo=.net&logoColor=white)




3. **Добавь то же самое** в начало `README.ru.md` для обратной навигации

Это стандартный подход в open-source проектах - держать переводы в отдельных файлах типа `README.ru.md`, `README.zh.md` и т.д.
