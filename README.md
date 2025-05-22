# AdPlacements – In‑Memory Ad Platform Matcher

A minimal **.NET 8** REST API that lets you upload a plain‑text list of advertising platforms with the regions where they operate and instantly query which platforms are valid for any location path.

---

##  Technologies used

| Purpose            | Tech / Package                                  |
| ------------------ | ----------------------------------------------- |
| Runtime & language | **.NET 8**, C# 12                               |
| Web layer          | **ASP.NET Core** controllers                    |
| DI & Logging       | Built‑in `Microsoft.Extensions.*`               |
| API docs           | **Swashbuckle** / Swagger UI                    |
| JSON               | **System.Text.Json** (no Newtonsoft dependency) |
| Tests              | **xUnit**, **Moq**, `Microsoft.NET.Test.Sdk`    |

---

##  Project structure

```
src/
 ├─ AdPlacements.Domain/        # Entities: Location, AdPlatform
 ├─ AdPlacements.Infrastructure/ # Parser + in‑memory repository
 ├─ AdPlacements.Application/   # Service + DTO
 └─ AdPlacements.Api/           # ASP.NET Core entry point
tests/
 └─ AdPlacements.Tests/         # xUnit specs
```

---

##  How it works

| Stage                       | Complexity                           | Notes                                                                                                         |
| --------------------------- | ------------------------------------ | ------------------------------------------------------------------------------------------------------------- |
| **Upload** (`POST /upload`) | **O(S)** time, **O(L)** memory       | Reads the file once, stores each platform exactly at its declared locations.                                  |
| **Search** (`GET /search`)  | **O(1)** time, **O(1)** extra memory | Walks the prefix chain of the requested path, merges results via `HashSet`. Depth ≤ 5 → effectively constant. |

---

##  Getting started

```bash
# Clone & run
git clone https://github.com/<your-org>/AdPlacements.git
cd AdPlacements/src/AdPlacements.Api
DOTNET_ENVIRONMENT=Development dotnet run
# Swagger: http://localhost:5000/swagger
```

> **Resetting data:** the service is RAM‑only – restart the process or re‑upload a file to clear state.

---

## 📑 File format

Each line: `PlatformName:location1[,location2…]`

```
Yandex.Direct:/ru
Revda Worker:/ru/svrd/revda,/ru/svrd/pervik
Cool Ads:/ru/svrd
```

* Locations start with `/` and may be nested: `/ru/svrd/revda` ⊂ `/ru/svrd` ⊂ `/ru`.

---

##  API reference

| Method   | Route                                     | Payload / Params                                                  | Response                                         |
| -------- | ----------------------------------------- | ----------------------------------------------------------------- | ------------------------------------------------ |
| **POST** | `/api/platforms/upload`                   | `multipart/form-data`  field **file** – text file described above | `204 No Content`                                 |
| **GET**  | `/api/platforms/search?location=/ru/svrd` | –                                                                 | JSON array of platform names, duplicates removed |

Example:

```bash
curl -F "file=@platforms.txt" http://localhost:5000/api/platforms/upload
curl "http://localhost:5000/api/platforms/search?location=/ru/svrd/ekb"
# → ["Yandex.Direct","Cool Ads"]
```

---

##  Running tests

```bash
# from repo root
dotnet test tests/AdPlacements.Tests
```

Tests cover the parser, repository indexing, and service logic.
