# HireMate

An Agentic Ai based application built with .NET Core.

---

## Prerequisites

Make sure the following are installed on your system before running the application:

- **Operating System**: Windows 10 or 11  
- **Database**: SQL Server 2022  
- **IDE**: Visual Studio 2022  
- **.NET SDK**: Compatible with the project (typically .NET 8 or later)

### Environment Setup

Create a `.env` file in the following directories:
- one env file in HireMate.Background.Scheduler (~\HireMate\Hiremate.background.schedule)
- one env file in HireMate (~\HireMate\HireMate)

properties in .env file <br>
Pass=your_gmail_app_password <br>
ApiKey=your_openrouter_api_key


---

## How to Run

1. **Clean and Build the Solution**
   - In Visual Studio: `Build > Clean Solution` and then `Build > Build Solution`
   - Or using CLI:
     
     dotnet clean
     dotnet build
     

2. **Update Database Migrations**
   - Run the following in the Package Manager Console:
     run command
     Update-Database
    

3. **Run the Background Scheduler**
   - Project: `HireMate.Background.Scheduler`
   - run the project from visual studio
    

4. **Run the Web Application**
   - Project: `HireMate`
   -  run the project from visual studio
    

---





