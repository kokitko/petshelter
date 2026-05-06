# 🐾 PetConnect: Warsaw Adoption Portal

A modern, high-performance Web API built with **.NET 10** and **Clean Architecture**. This platform connects local shelters (Organizations) and citizens (Users) to streamline the pet adoption process in a localized urban environment (optimized for Warsaw).

---

## 🏗 Architectural Overview

The project is built with a focus on maintainability, scalability, and performance.

*   **Clean Architecture**: Separation of concerns between `Domain`, `Application`, `Infrastructure`, and `API` layers.
*   **CQRS Pattern**: Implemented via **MediatR** to decouple read and write operations.
*   **Functional Error Handling**: Uses the **ErrorOr** library. Instead of throwing expensive exceptions, the domain logic returns error objects which are then handled by a global controller mapping.
*   **MediatR Pipeline Behaviors**:
    *   **Automated Caching**: A custom pipeline intercepts Queries to handle **Redis caching** globally, ensuring high performance without duplicating code in handlers.
    *   **Validation**: Automatic request validation using **FluentValidation** before any business logic is executed.
*   **Global Exception Handling**: A centralized middleware handles unpredicted failures, returning consistent, RFC-compliant responses.

## 🛠 Tech Stack

| Category | Technology |
| :--- | :--- |
| **Framework** | .NET 8 (ASP.NET Core) |
| **Database** | MS SQL Server with Entity Framework Core |
| **Caching** | Redis (Distributed Cache) |
| **Mapping** | **Mapperly** (Source-generator based, superior performance) |
| **Docs** | **Scalar** (OpenAPI 3.1) |
| **Containerization** | Docker & Docker Compose |
| **Auth** | JWT (Access & Refresh Tokens) |

## ⚙️ Business Logic & Workflow

### User Roles
*   **Users & Organizations**: Both can list pets and apply for adoption. The system is designed for local interactions (location is implicit for the city).
*   **Administrator**: System oversight, including moderation and administrative actions.

### Adoption Lifecycle
1.  **Available**: Pet is listed and open for applications.
2.  **Pending**: An organization/owner has **Approved** an application. The pet is reserved.
3.  **Adopted**: The applicant confirms the handover.
4.  **Rollback**: If the applicant rejects the process, the pet's status automatically resets to **Available**.

## 🚀 Getting Started

### Prerequisites
*   Docker Desktop
*   .NET 10 SDK

### Installation & Run
1.  **Clone the repository**:
    ```bash
    git clone [https://github.com/kokitko/petshelter.git](https://github.com/kokitko/petshelter.git)
    ```
2.  **Spin up Infrastructure and run the project**:
    This starts SQL Server and Redis via Docker, also runs the application.
    ```bash
    docker-compose up --build -d
    ```
3.  **Database Initialization**:
    The API includes a **Smart Seeder** that populates the DB with test data, including complex edge-case scenarios and different pet statuses.

## 📖 API Documentation
Once the app is running, the interactive **Scalar** documentation is available at:
`http://localhost:8080/scalar/v1`

> **Note:** Documentation includes detailed request/response schemas and specific error codes (via ErrorOr) for every endpoint.

## 🚧 Roadmap
- [ ] **Azure Blob Storage**: Moving from the current local file storage stub to Azure for avatars and pet photos.
- [ ] **Tests**: Integration tests for general application testing, Unit-tests for business-logic checking.
- [ ] **Front-End**: Front-End, so application is usable by non-developers.
- [ ] **CI/CD**: Setting up CI/CD pipelines for automated testing and deployment.
- [ ] **Deployment**: Deploying the application to a VPS!

---
*Developed by [kokitko](https://github.com/kokitko)*
