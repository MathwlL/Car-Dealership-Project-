
# CarMark Dealership Project

A fictional car dealership web application developed with .NET, designed to provide a vehicle catalog, search functionality, and a user authentication area.

This project aims to demonstrate web development concepts, database integration, application architecture, and security best practices within the .NET ecosystem.

> **Project Status:** In Development  
> **Project Type:** Academic Project / Portfolio  
> **Domain:** Automotive Sales

---

##  Table of Contents

- [About the Project](#-about-the-project)
- [Project Goals](#-project-goals)
- [Technologies](#-technologies)
- [Features](#-features)
- [User Interface](#-user-interface)
- [Database](#-database)
- [Project Structure](#-project-structure)
- [Getting Started](#-getting-started)
- [Security Review](#-security-review)
- [Future Improvements](#-future-improvements)
- [Author](#-author)

---

##  About the Project

The Car Dealership Project is a web application that simulates the basic operations of a fictional car dealership.

The main goal is to provide a user-friendly interface where visitors can explore available vehicles, search for specific models, and view relevant vehicle information.

The project also includes plans for a login area, which can support authenticated users and future administrative features.

The application is intended to evolve into a maintainable and secure system, following good software engineering practices.

## 🎯 Project Goals

- Develop a web application using the .NET ecosystem.
- Organize the codebase for maintainability and scalability.
- Provide a vehicle catalog.
- Implement vehicle search and filtering.
- Create a user authentication area.
- Integrate a database for persistent data storage.
- Apply input validation and access control.
- Follow security best practices throughout development.

##  Technologies

The following technologies are relevant to the project. The actual dependencies should be confirmed against the source code.

| Technology | Purpose |
|---|---|
| C# | Main programming language |
| .NET / ASP.NET Core | Web application development |
| Entity Framework Core | Object-relational mapping |
| SQL Server or SQLite | Database storage, depending on configuration |
| HTML, CSS, JavaScript | User interface |
| Bootstrap | Responsive styling |
| Git and GitHub | Version control and source code hosting |

---

##  Features

### 1. Home Page

The Home page serves as the main entry point of the application, introducing the dealership and providing access to the main features.

**Planned features:**

- Dealership introduction and branding.
- Featured vehicles.
- Navigation to the vehicle search page.
- Quick access to the login area.
- Highlights of available cars.

**Screenshot**

<!-- Add a screenshot of the Home page here -->

![Home Page](https://github.com/MathwlL/Car-Dealership-Project-/blob/f315b5a39c075b21371fb2cc447d7772d6daf856/Github%20assets/%7B5923AABF-45F2-47BC-AED1-47BECB305196%7D.png)

### 2. Vehicle Search

The Vehicle Search page allows visitors to browse the catalog and find vehicles based on their preferences.

**Planned features:**

- Search by brand and model.
- Filter by year, price, and vehicle category.
- Display vehicle listings.
- View detailed vehicle information.
- Display a message when no results are found.

**Suggested vehicle information:**

- Brand and model.
- Manufacturing year and model year.
- Price.
- Mileage.
- Fuel type.
- Transmission.
- Images and description.

**Screenshot**


![Vehicle Search](https://github.com/MathwlL/Car-Dealership-Project-/blob/f315b5a39c075b21371fb2cc447d7772d6daf856/Github%20assets/%7B96D13862-C12A-46DB-8B52-80D45ADCF1A8%7D.png)
![Vehicle Search](https://github.com/MathwlL/Car-Dealership-Project-/blob/f315b5a39c075b21371fb2cc447d7772d6daf856/Github%20assets/%7B81334D5E-78FE-4F07-A376-FF9ED3FC52FE%7D.png)

### 3. Login and Authentication

The Login page provides an entry point for users who need access to restricted features.

**Planned features:**

- Login form.
- Server-side credential validation.
- Appropriate error messages.
- Secure session or authentication cookie handling.
- Logout functionality.
- Access restrictions for protected pages and operations.

Registration, password recovery, and role-based access control may be added as the application evolves.

**Important:** A login page alone does not secure an application. Authentication and authorization must also be enforced on the server.

**Screenshot**

<!-- Add a screenshot of the Login page here -->

![Login Page](https://github.com/MathwlL/Car-Dealership-Project-/blob/f315b5a39c075b21371fb2cc447d7772d6daf856/Github%20assets/%7B61D129CC-F376-4450-8FA7-1B08790C78BB%7D.png)

---

##  Database

The database is intended to store the information required by the application, including vehicle details and, if implemented, user account information.

The actual database engine, schema, and data access approach must be verified against the project configuration.

### Proposed Data Model

The following entities represent a possible future design and are not a confirmed description of the current database.

| Entity | Responsibility |
|---|---|
| `Vehicles` | Stores vehicle information |
| `Users` | Stores information required for user accounts |
| `Categories` | Organizes vehicles by category, if needed |
| `VehicleImages` | Stores or references vehicle images, if needed |

---

### Main Components

- **Controllers:** Handle incoming HTTP requests and coordinate responses.
- **Models:** Represent application entities and data structures.
- **Data:** Contains database access components and the database context.
- **Services:** Encapsulate business logic when a service layer is used.
- **Views:** Contain server-rendered pages in an MVC application.
- **wwwroot:** Stores static assets such as CSS, JavaScript, and images.
- **Program.cs:** Configures application services and the HTTP request pipeline.

The actual structure depends on whether the project uses MVC, Razor Pages, a Web API, or a separate frontend.

---

##  Getting Started

### Prerequisites

Before running the application, make sure you have:

- A compatible .NET SDK.
- Git installed.
- A configured database, if required by the application.
- Visual Studio, Visual Studio Code, or another compatible development environment.

### 1. Clone the Repository

```bash
git clone https://github.com/MathwlL/Car-Dealership-Project-.git
cd Car-Dealership-Project-
```

### 2. Restore Dependencies

```bash
dotnet restore
```

If the solution or project file is located in a subdirectory, run the command from the appropriate directory or specify the project path.

### 3. Configure the Database

Configure the database connection according to the database engine used by the project.

Never commit production credentials, passwords, or secret keys to version control.

If the project uses Entity Framework Core and has migrations configured, update the database with:

```bash
dotnet ef database update
```

This command requires the appropriate EF Core tooling, dependencies, and existing migrations.

### 4. Run the Application

```bash
dotnet run
```

Open the local URL displayed in the terminal.

> The commands above are general .NET instructions. The exact setup may vary depending on the project's current configuration.

---

##  Author

**Author:** `[Your Name or GitHub Username]`

**Repository:** [Car Dealership Project](https://github.com/MathwlL/Car-Dealership-Project-)

---

##  License

No license has been specified in this documentation.

---

*This project is intended for educational and portfolio purposes, focusing on .NET web development, database integration, and software engineering best practices.*
