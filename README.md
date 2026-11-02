# Gift of the Givers Foundation

## 1. Project Overview

The Gift of the Givers Foundation application is an ASP.NET Core MVC web application prototype designed to support disaster relief activities.

The application aims to provide a platform where employees can share relief project updates, donors can make contributions, and members of the public can register their interest in volunteering.

## 2. Main Features

The project includes the following application features:

- User registration and login.
- Employee and donor access.
- Relief project management and progress updates.
- Donation forms supporting once-off and recurring donation options.
- Donation currency options, including ZAR, USD and EUR.
- Anonymous donation option.
- Tax certificate demonstration functionality.
- Volunteer registration.
- Responsive web interface using Bootstrap.

Some functions are prototype demonstrations and should not be interpreted as live financial or payment-processing services.

## 3. Technologies Used

| Technology | Purpose |
|---|---|
| C# | Main application programming language |
| ASP.NET Core MVC | Web application framework |
| .NET 10 | Main web application target framework |
| ASP.NET Core Identity | User authentication and account management |
| Entity Framework Core | Data access and database operations |
| Azure SQL Database | Cloud database |
| Azure Functions (.NET 8) | Serverless project component |
| Bootstrap | Responsive user interface |
| Selenium WebDriver | Automated browser testing |
| MSTest | Automated test framework |
| Azure DevOps | Pipeline configuration |
| Git and GitHub | Source control and version management |

## 4. Solution Structure

The solution contains four main projects:

| Project | Description |
|---|---|
| `GiftOfTheGivers` | Main ASP.NET Core MVC web application |
| `GiftOfTheGivers.Functions` | Azure Functions project |
| `GiftOfTheGivers.Helpers` | Shared helper functionality |
| `GiftOfTheGivers.Tests` | Automated testing project |

Additional project files include:

- `azure-pipelines.yml` – Azure DevOps pipeline configuration.
- `Database/Scripts/` – SQL database deployment scripts.
- `AZURE_DEPLOYMENT_LIMITATIONS.txt` – Record of Azure subscription and deployment restrictions.

## 5. Running the Application Locally

### Prerequisites

The following software is required:

- Visual Studio 2022 or a compatible version supporting the project's .NET SDK.
- .NET 10 SDK.
- Git.

### Instructions

1. Clone the GitHub repository:

   ```powershell
   git clone https://github.com/tirisanomolaolwe-byte/GiftOfTheGivers-Foundation.git
   ```

2. Open `GiftOfTheGivers.slnx` in Visual Studio.

3. Allow Visual Studio to restore the required NuGet packages.

4. Set `GiftOfTheGivers` as the startup project.

5. Select Run or press F5 to launch the application.

The application has previously been run locally at:

`http://localhost:7150`

The actual local address may differ depending on the launch configuration.

## 6. Running Automated Tests

The solution includes the `GiftOfTheGivers.Tests` project.

To execute the tests in Visual Studio:

1. Open the solution.
2. Select **Test** from the Visual Studio menu.
3. Open **Test Explorer**.
4. Select **Run All Tests**.
5. Review the test results.

Alternatively, run the following command from the solution directory:

```powershell
dotnet test GiftOfTheGivers.slnx
```

Successful local test execution confirms that the tests ran in the local development environment. It does not by itself confirm a successful Azure DevOps pipeline run.

## 7. Database Schema Script

The repository contains the following SQL script:

`Database/Scripts/001_Create_ReliefProjectUpdates.sql`

The script creates the `dbo.ReliefProjectUpdates` table, which stores relief project progress updates.

The table includes:

- UpdateId
- ProjectId
- UpdateTitle
- UpdateDescription
- UpdateDate
- UpdatedBy

The script checks whether the table already exists before attempting to create it.

The schema update was manually executed and verified in the Azure SQL Database using the Azure Portal Query Editor.

## 8. Azure DevOps Pipeline

The project includes an `azure-pipelines.yml` file to configure automated build and test activities.

The pipeline is configured to use the GitHub repository and the `main` branch.

A hosted pipeline run was blocked by the Azure DevOps organisation's hosted parallelism restriction. The error indicated that no hosted parallelism had been purchased or granted.

The pipeline configuration is included in the repository, but a successful hosted pipeline execution has not been confirmed.

## 9. Azure Deployment Limitations

Certain cloud deployment activities were affected by Azure subscription restrictions and account permissions.

These included:

- Insufficient Microsoft Entra permissions to create an application registration for an Azure Resource Manager service connection.
- Insufficient permissions to create a user-assigned managed identity.
- Azure subscription policy restrictions on the permitted App Service Plan SKUs when creating the Azure Function App.
- Lack of hosted parallelism for Azure DevOps pipeline execution.

Further details are provided in:

`AZURE_DEPLOYMENT_LIMITATIONS.txt`

The local application testing and manual Azure SQL schema update were completed separately from the cloud deployment activities that remained blocked.

## 10. Source Code Repository

GitHub repository:

https://github.com/tirisanomolaolwe-byte/GiftOfTheGivers-Foundation

The repository contains the application source code, automated test project, Azure pipeline configuration and SQL deployment script.

## 11. Project Documentation

The project handover package includes:

- Application User Manual.
- Team Contribution Report.
- Azure Deployment Limitations document.
- Supporting screenshots and Git evidence.
