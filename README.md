# 🗳️ VotingAPI

VotingAPI is a simple, lightweight RESTful API built with ASP.NET Core for managing polls and votes. It supports creating polls, casting votes, and retrieving results. Ideal for learning, testing, or using as a foundation for more complex voting systems.

---

## 📦 Table of Contents

- [Features](#features)
- [Tech Stack](#tech-stack)
- [Setup & Installation](#setup--installation)
- [Configuration](#configuration)
- [Running the API](#running-the-api)
- [API Endpoints](#api-endpoints)
- [Database](#database)
- [Testing](#testing)
- [Contributing](#contributing)
- [License](#license)

---

## 🎯 Features

- Create new polls with multiple choices.
- Cast votes on existing polls.
- View poll results and vote counts.
- RESTful endpoint design.
- Input validation and error handling.
- In-memory or persistent storage options (e.g., SQLite, SQL Server).
- (Optional) JWT-based authentication and authorization.

---

## 🧰 Tech Stack

- 🛠️ **Framework**: .NET 7+ / .NET Core
- 🌐 **Web API**: ASP.NET Core Web API
- 📦 **ORM**: Entity Framework Core (In-Memory / SQLite)
- 🧪 **Testing**: xUnit
- (Optional) 🔐 **Auth**: JWT Token Authentication

---

## 🚀 Setup & Installation

**Prerequisites:**

- [.NET 7 SDK](https://dotnet.microsoft.com/download)
- Optional: Docker (if using containerized DB)

1. Clone the repository

   ```bash
   git clone https://github.com/MRomarr/VotingAPI.git
   cd VotingAPI
