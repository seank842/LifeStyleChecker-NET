# 🧬 LifeStyleChecker-NET

**LifeStyleChecker-NET** is a modular Aspire-powered .NET solution designed to assess lifestyle eligibility through customizable rules, distributed caching, and streamlined validation. It orchestrates a Blazor Web frontend, API service, and foundational services using .NET Aspire for isolated deployment and service discovery.

---

## ⚙️ Project Structure
```
LifeStyleChecker-NET/
├─ LifestyleChecker.AppHost                    # Aspire service orchestrator
├─ LifestyleChecker.Web                        # Blazor Web frontend
├─ LifestyleChecker.ApiService                 # REST API and endpoints
├─ LifestyleChecker.Contracts.DTOs             # Shared data contracts
├─ LifestyleChecker.DTOs                       # Transfer objects
├─ LifestyleChecker.Domain                     # Core business logic
├─ LifestyleChecker.Infrastructure.Persistence # EF Core + caching
├─ LifestyleChecker.SharedUtilities            # Extensions
├─ LifestyleChecker.ServiceDefaults            # Central service configurations
├─ LifestyleChecker.Tests                      # Unit testing
├─ Services                                    # Lifestyle validation rules
```

---

## 🧠 Key Features

- 🔒 **Custom Authentication Attributes**
  - `PatientAuthorize` and `AdminAuthorize` offer declarative access control.
  - Integrated into API and Blazor endpoints for secure route enforcement.

- 📦 **Redis with Intelligent Fallback**
  - Redis used as primary caching mechanism.
  - Automatically fails over to in-memory caching when Redis is unavailable.
  - Connectivity checks prevent deadlock or cascading failures.

- 🧰 **Centralized Validation Services**
  - Validators support composition and chaining for complex assessments.
  - Bitwise flags for eligibility scoring.

- 🧪 **Shared Service Defaults**
  - Consolidated config for CORS, Swagger, JSON serialization and error handling.
  - Enhances maintainability across microservice boundaries.

- 🧠 **Blazor Web Frontend**
  - Dynamic UI rendered in Interactive Server.
  - Components integrated with questionnaire rule validators and API services.

---

## 🔄 Redis Fallback Logic

```csharp
var redisHealthy = redisProvider.Ping();
var cache = redisHealthy ? redisProvider : fallbackCache;
```
- Pings Redis before executing reads/writes.
- Transparent fallback to MemoryCache.
- Logging included for fallback detection and diagnostics.

## 🔒 Authorisation Attributes
`[PatientAuthorise]`
```csharp
[PatientAuthorise]
public async Task<ActionResult<PatientDTO>> GetCurrentUser() { ... }
```
- Enforces patient-level access rules.
- Supports extension to conditionally allow read/write operations.
  
`[AdminAuthorise]`
```csharp
[AdminAuthorise]
public ActionResult AdminAuthCheck() { ... }
```
- Restricts access to administrative operations.
- Validation logic tied to claims evaluation.

# 🧠 Rule Validation Pipeline
- Rules evaluated individually or composed via bitwise logic.
- Validators injected via DI for easy testing and reuse.

# 🧪 Testing
- LifestyleChecker.Tests contains BDD Tests for Correct form Evaluation and Patient Loading
- xUnit and Xunit.Gherkin.Quick used as the test framework.

# 🧭 Getting Started
```Bash
git clone https://github.com/seank842/LifeStyleChecker-NET
cd LifeStyleChecker-NET

# Optional: launch Redis locally
docker run -p 6379:6379 -d redis

dotnet restore
dotnet build

# Run using Aspire AppHost
dotnet run --project LifestyleChecker.AppHost
```

# 🌱 Future Enhancements
- Telemetry integration for rule evaluations
- Role-based UI workflows
- Advanced tracing and diagnostics across services
