# Project Correspondence

A full-stack web application for project management with milestone tracking, featuring JWT token authentication and role-based access control.

## Features

### Authentication & Authorization
- **JWT Token Authentication**: Secure token-based authentication
- **Role-Based Access Control**: Three user roles - Admin, ProjectManager, and User
- **User Registration & Login**: Complete user management system

### Project Management
- **Create Projects** with:
  - Project name
  - Project link
  - Description
  - Approver name
  - Status tracking (Pending, InProgress, Completed, Rejected)
  - Number of milestones
  - File attachments support
- **View All Projects**: List view with filtering and status badges
- **Project Details**: Comprehensive project information display
- **Update Projects**: Edit project details
- **Delete Projects**: Remove projects (Admin/ProjectManager only)

### Milestone Tracking
- **Create Milestones**: Add milestones to projects
- **Track Achievement**: Mark milestones as achieved with timestamps
- **Approval Workflow**: 
  - ProjectManagers and Admins can approve/reject milestones
  - Add approval comments
  - Track approval history
- **Progress Visualization**: Visual progress bars and statistics

## Technology Stack

### Backend
- **ASP.NET Core 8.0** - Web API
- **Entity Framework Core** - ORM with SQL Server
- **ASP.NET Identity** - User management
- **JWT Bearer Authentication** - Token-based auth
- **Swagger/OpenAPI** - API documentation

### Frontend
- **Blazor WebAssembly** - SPA framework
- **Bootstrap 5** - UI components
- **Blazored.LocalStorage** - Client-side storage

### Database
- **SQL Server** (LocalDB for development)

## Project Structure

```
project-correspondence/
├── Backend/                    # ASP.NET Core Web API
│   ├── Controllers/           # API endpoints
│   │   ├── AuthController.cs
│   │   ├── ProjectsController.cs
│   │   └── MilestonesController.cs
│   ├── Data/                  # Database context
│   │   └── ApplicationDbContext.cs
│   ├── Models/                # Entity models
│   │   ├── ApplicationUser.cs
│   │   ├── Project.cs
│   │   ├── Milestone.cs
│   │   └── ProjectAttachment.cs
│   └── Program.cs             # API configuration
│
├── Frontend/                   # Blazor WebAssembly
│   ├── Pages/                 # Razor components
│   │   ├── Index.razor
│   │   ├── Login.razor
│   │   ├── Register.razor
│   │   ├── Projects.razor
│   │   ├── CreateProject.razor
│   │   ├── ProjectDetails.razor
│   │   └── ProjectMilestones.razor
│   ├── Services/              # HTTP services
│   │   ├── AuthService.cs
│   │   ├── ProjectService.cs
│   │   ├── MilestoneService.cs
│   │   └── CustomAuthStateProvider.cs
│   ├── Shared/                # Shared components
│   │   ├── MainLayout.razor
│   │   └── NavMenu.razor
│   └── Program.cs             # WASM configuration
│
└── Shared/                     # Shared DTOs
    └── DTOs/                  # Data Transfer Objects
        ├── AuthDTOs.cs
        ├── ProjectDTOs.cs
        └── MilestoneDTOs.cs
```

## Prerequisites

Before running this application, ensure you have the following installed:

- **.NET 8.0 SDK** or later - [Download](https://dotnet.microsoft.com/download)
- **SQL Server** or **SQL Server LocalDB**
- **Visual Studio 2022** (optional, but recommended) or **VS Code**

## Getting Started

### 1. Clone the Repository

```bash
git clone <repository-url>
cd project-correspondence
```

### 2. Configure the Database Connection

Edit `Backend/appsettings.json` and update the connection string if needed:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ProjectCorrespondenceDB;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### 3. Apply Database Migrations

Navigate to the Backend directory and run:

```bash
cd Backend
dotnet ef migrations add InitialCreate
dotnet ef database update
```

> **Note**: If you don't have EF Core tools installed, run:
> ```bash
> dotnet tool install --global dotnet-ef
> ```

### 4. Update JWT Settings (Optional)

For production, update the JWT secret key in `Backend/appsettings.json`:

```json
{
  "JwtSettings": {
    "SecretKey": "YourVerySecureSecretKeyThatIsAtLeast32CharactersLong!",
    "Issuer": "ProjectCorrespondenceAPI",
    "Audience": "ProjectCorrespondenceClient",
    "ExpiryMinutes": 60
  }
}
```

### 5. Run the Backend API

From the Backend directory:

```bash
dotnet run
```

The API will be available at:
- HTTPS: `https://localhost:7000`
- HTTP: `http://localhost:5000`
- Swagger UI: `https://localhost:7000/swagger`

### 6. Run the Frontend

Open a new terminal and navigate to the Frontend directory:

```bash
cd Frontend
dotnet run
```

The frontend will be available at:
- HTTPS: `https://localhost:7001`
- HTTP: `http://localhost:5001`

### 7. Access the Application

Open your browser and navigate to `https://localhost:7001`

## Default Roles

The application automatically creates three roles on startup:
- **Admin** - Full access to all features
- **ProjectManager** - Can approve milestones and manage projects
- **User** - Can create projects and milestones

## API Endpoints

### Authentication
- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - Login user

### Projects
- `GET /api/projects` - Get all projects
- `GET /api/projects/{id}` - Get project by ID
- `POST /api/projects` - Create new project
- `PUT /api/projects/{id}` - Update project
- `DELETE /api/projects/{id}` - Delete project (Admin/PM only)
- `POST /api/projects/{id}/attachments` - Upload attachment

### Milestones
- `GET /api/milestones/project/{projectId}` - Get milestones by project
- `GET /api/milestones/{id}` - Get milestone by ID
- `POST /api/milestones` - Create new milestone
- `PUT /api/milestones/{id}` - Update milestone
- `POST /api/milestones/{id}/approve` - Approve/reject milestone (Admin/PM only)
- `DELETE /api/milestones/{id}` - Delete milestone (Admin/PM only)

## Usage Guide

### 1. Register a New User
1. Navigate to the Register page
2. Fill in your details (Full Name, Email, Password)
3. Select your role (User, ProjectManager, or Admin)
4. Click "Register"

### 2. Create a Project
1. Login to your account
2. Click "Create New Project" or navigate to `/projects/create`
3. Fill in the project details:
   - Project Name (required)
   - Project Link (optional)
   - Description (optional)
   - Approver Name (required)
   - Number of Milestones (required)
4. Click "Create Project"

### 3. Add Milestones
1. Navigate to a project's detail page
2. Click "Manage Milestones"
3. Enter milestone name and optional description
4. Click "Add"
5. Repeat for all milestones

### 4. Track Milestone Progress
1. On the milestones page, click "Mark Achieved" for completed milestones
2. ProjectManagers and Admins can approve milestones by clicking "Approve"
3. Add optional approval comments
4. Select Approve or Reject

## Development

### Building for Production

#### Backend
```bash
cd Backend
dotnet publish -c Release -o ./publish
```

#### Frontend
```bash
cd Frontend
dotnet publish -c Release -o ./publish
```

### Running Tests

```bash
dotnet test
```

## Security Considerations

1. **Change JWT Secret**: Update the JWT secret key in production
2. **HTTPS Only**: Ensure HTTPS is enforced in production
3. **CORS Configuration**: Update CORS policy for your production domain
4. **Connection Strings**: Use secure connection strings with proper credentials
5. **Password Policy**: Configure password requirements in `Program.cs`

## Troubleshooting

### Database Connection Issues
- Ensure SQL Server LocalDB is installed and running
- Check the connection string in `appsettings.json`
- Run migrations: `dotnet ef database update`

### CORS Errors
- Verify the frontend URL in `Backend/Program.cs` CORS configuration
- Ensure both projects are running on the correct ports

### Authentication Issues
- Check that the JWT secret key matches in both projects
- Verify token expiration settings
- Clear browser local storage and re-login

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License.

## Support

For issues and questions, please open an issue on the GitHub repository.
