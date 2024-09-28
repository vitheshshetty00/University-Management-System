# University Management System

The University Management System is a comprehensive application designed to manage various aspects of a university, including students, faculty, courses, and payments. This project is built using C# and targets .NET 8.0.

## Table of Contents

- [Features](#features)
- [Technologies Used](#technologies-used)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
- [Usage](#usage)
- [Project Structure](#project-structure)

## Features

- **Student Management**: Add, remove, and display students.
- **Faculty Management**: Add, remove, and display faculty members.
- **Course Management**: Add, remove, and display courses.
- **Payment Processing**: Process payments via credit card or bank transfer.
- **Database Management**: Automatically create and migrate the database.

## Technologies Used

- **C# 12.0**
- **.NET 8.0**
- **Entity Framework Core 8.0.8**
- **SQL Server**
- **Microsoft.Extensions.DependencyInjection**

## Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

### Installation

1. **Clone the repository:**

    ```bash
    git clone https://github.com/your-username/university-management-system.git
    cd university-management-system
    ```

2. **Set up the database connection string:**

    Update the `app.config` file with your SQL Server connection string.

    ```xml
    <connectionStrings>
        <add name="UniversityDbContext"
             connectionString="Connection string here" 
             providerName="System.Data.SqlClient" />
    </connectionStrings>
    ```

3. **Run the application:**

    ```bash
    dotnet run --project "University Management System/University Management System.csproj"
    ```

## Usage

Once the application is running, you can interact with it via the console. The menu will guide you through various operations such as adding/removing students, faculty, and courses, as well as processing payments.

## Project Structure
```
University Management System
├── Data/
|   ├── UniversityDbContext.cs
│
└── Migrations/
├── Entities/
│ ├── Student.cs
│ ├── Faculty.cs
│ ├── Course.cs
│ ├── Address.cs
│ └── Payment.cs
├── PaymentGateways/
│ ├── PaymentGateway.cs
│ ├── CreditCardPaymentGateway.cs
│ └── BankTransferPaymentGateway.cs
├── Exceptions/
│ ├── StudentNotFoundException.cs
│ ├── CourseNotFoundException.cs
│ └── FacultyNotFoundException.cs
├── Services/
│ ├── Interfaces/
│ │ ├── IStudentService.cs
│ │ ├── IFacultyService.cs
│ │ ├── ICourseService.cs
│ │ └── IPaymentService.cs
│ └── Implementations/
│ │  ├── DbStudentService.cs
│ │  ├── DbFacultyService.cs
│ │  ├── DbCourseService.cs
│ │  ├── DbPaymentService.cs
│    └── MenuService.cs
├── Program.cs
├── app.config
└── University Management System.csproj

```


    
