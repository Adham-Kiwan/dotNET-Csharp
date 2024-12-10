# To-Do List API

## Description

This project is a backend API for a simple To-Do List application, implementing full CRUD (Create, Read, Update, Delete) functionality. It allows users to manage their tasks, with features to add, view, update, and delete to-do items. Built using .NET, the API provides endpoints to interact with a SQLite database, storing user tasks and supporting basic authentication.

## Table of Contents

- [Description](#description)
- [Installation](#installation)
- [Configuration](#configuration)
- [Usage](#usage)
- [Contributing](#contributing)

## Installation

Follow the steps below to set up the project on your local machine.

### Prerequisites

Before running this project, make sure you have the following installed:

- .NET SDK (version 9.0.0 or above)
- SQLite (if using SQLite as a database)

### Steps to Install

1. Clone the repository:
    ```bash
    git clone git@github.com:Adham-Kiwan/dotNET-Csharp.git
    cd dotNET-Csharp
    ```

2. Install the necessary dependencies:
    ```bash
    dotnet restore
    ```

### NuGet Packages

This project uses the following NuGet packages:

- **Bondx.MinimalApi.Extensions**: For minimal API extensions in .NET.
- **Microsoft.AspNetCore.Authentication.JwtBearer**: For JWT bearer authentication.
- **Microsoft.EntityFrameworkCore.Design**: For Entity Framework Core design-time tools.
- **Microsoft.EntityFrameworkCore.Sqlite**: For SQLite support in Entity Framework Core.
- **MinimalApis.Extensions**: For additional minimal API extensions.
- **System.IdentityModel.Tokens.Jwt**: For JWT token creation and validation.

## Configuration

### Create `appsettings.json`

You need to create a `appsettings.json` file in the root directory. Below is a template you can use:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore.Database.Command": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "ToDosStore": "Data Source=TodoList.db"
  },

  "JwtSettings": {
    "SecretKey": "your-secret-key-here", 
    "Issuer": "your-app-name",     //use: "your-app"
    "Audience": "your-app-name"     //use: "your-app"
  }
}
```
Don't forget to update the new token you got after logging in in the testing file and in necessary places.
To know which email to use when logging in, you can see the 20241208190501_SeedUsers.cs file, which seeds in 3 hardcoded users into the database.

## Usage

After configuring your `appsettings.json` or setting the environment variables, you can run the application with the following command:

```bash
dotnet run
```


### Testing the API

To test the API, you can use tools like [Postman](https://www.postman.com/) or [curl](https://curl.se/), or use the file i included in the project: ToDo.http.

#### Authentication

For endpoints that require authentication, include a valid JWT token in the Authorization header with the prefix `Bearer`. Example:

Replace `<your-token>` with a valid JWT token that you can obtain by logging in through the authentication endpoint.


## Contributing Section

```markdown
## Contributing

We welcome contributions to this project!
```



