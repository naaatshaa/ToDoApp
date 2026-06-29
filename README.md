# ToDoApp

REST API для управления списком задач.

---

## Стек

- .NET 10 / ASP.NET Core
- Entity Framework Core
- PostgreSQL
- Swagger / OpenAPI
- Docker / Docker Compose
- xUnit + Moq

---

## Запуск

### Локально

Требования: .NET 10 SDK, PostgreSQL.

1. Создать базу данных `TodoDb`.
2. Указать строку подключения в `appsettings.json`:

{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=TodoDb;Username=postgres;Password=..."
  }
}

3. Выполнить:

dotnet restore
dotnet run

4. Swagger доступен по адресу:

http://localhost:5000/swagger

---

### Через Docker Compose

docker-compose up --build

После запуска API и PostgreSQL поднимаются вместе. Swagger доступен по тому же адресу http://localhost:5000/swagger.

---

### Только PostgreSQL в Docker, API локально

docker run --name postgres \
  -e POSTGRES_USER=postgres \
  -e POSTGRES_PASSWORD=postgres \
  -e POSTGRES_DB=TodoDb \
  -p 5432:5432 \
  -d postgres:16

dotnet run

---

## API

| Метод | URL | Описание |
|-------|-----|----------|
| GET | /api/Tasks | Получить все задачи |
| GET | /api/Tasks/{id} | Получить задачу по ID |
| POST | /api/Tasks | Создать задачу |
| PUT | /api/Tasks/{id} | Обновить задачу |
| DELETE | /api/Tasks/{id} | Удалить задачу |

---

## Структура решения

- `ToDoApp/Controllers/TasksController.cs`
- `ToDoApp/Data/AppDbContext.cs`
- `ToDoApp/Data/TaskRepository.cs`
- `ToDoApp/Models/TaskItem.cs`
- `ToDoApp/Services/TaskService.cs`
- `ToDoApp.Tests/TaskRepositoryTests.cs`
- `ToDoApp.Tests/ToDoApp.Tests.csproj`
- `Program.cs`
- `Dockerfile`
- `docker-compose.yml`
- `README.md`

---

## Тесты

Запуск:

dotnet test

Используются xUnit и InMemoryDatabase для изоляции тестов от реальной БД.

---

## Архитектура

Проект разделён на три слоя:

- Controller — обработка HTTP-запросов, валидация.
- Service — бизнес-логика.
- Repository — доступ к данным через EF Core.

Это позволяет изолировать изменения: замена ORM или способа хранения не затрагивает контроллеры.

---

## Обоснование выбора инструментов

- Entity Framework Core — выбран для ускорения разработки за счёт миграций и автоматического маппинга. Для проекта такого объёма это оправдано.
- Репозиторий — добавлен как абстракция над EF Core. При необходимости можно заменить на Dapper или прямые SQL-запросы без изменения сервисного слоя.
- Docker — обеспечивает воспроизводимость окружения и упрощает проверку проекта на разных машинах.

---

## Возможные улучшения

- Добавить аутентификацию (JWT) и привязку задач к пользователям.
- Реализовать пагинацию и фильтрацию в GET-запросах.
- Внедрить структурированное логирование (Serilog).
- Написать интеграционные тесты для контроллеров.
