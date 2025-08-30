#  Book Management App

A sample project built with **.NET 9** demonstrating CRUD operations for managing books.

##  Requirements

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)  
- SQL Server
- (Optional) Visual Studio 2022 / VS Code  

##  Setup Instructions

1. **Clone the repository**
   ```bash
   git clone git@github.com:thetngyonphoo/book-management-api.git

   ```

2. **Update database connection string**  
   Open `appsettings.json` in the `BookManagementApp.API` project and set your database connection string.

3. **Apply EF Core migrations**
   ```bash
   dotnet ef database update --project BookManagementApp.Infrastructure --startup-project BookManagementApp.API
   ```

4. **Run the project**
   ```bash
   dotnet run --project BookManagementApp.API
   ```

5. **Access the API**
   
   - Scalar UI : [https://localhost:7284/scalar](https://localhost:7284/scalar)  

##  Features

- Create, Read, Update, Delete (CRUD) operations for books  
- Category management via enums  
- Repository pattern with clean architecture  
- Logging and exception handling  

##  Project Structure

```
BookManagementApp
├── BookManagementApp.API            # API Layer
├── BookManagementApp.Domain         # Entities & Enums
├── BookManagementApp.Service        # Business Logic
├── BookManagementApp.Infrastructure # Data Access (EF Core)
```


