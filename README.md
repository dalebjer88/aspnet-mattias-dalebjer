# CoreFitnessClub

This project was created as part of my Web Developer .NET education at EC Utbildning. The assignment focuses on building an ASP.NET Core MVC application based on a provided Figma design, using Identity, Entity Framework Core, layered architecture, validation, authentication, authorization, and tests. The goal is to meet the requirements for both a passing grade (G) and a higher grade (VG), while keeping the solution structured, readable, and aligned with the architectural requirements of the assignment.

The application represents a fictional fitness club where users can create an account, sign in, manage their profile, choose a membership, view available training classes, book and cancel classes, and manage their account. Admin users can create and delete training classes. The project also includes a customer service contact form and pages based on the provided design.

The solution is organized into separate projects for Domain, Application, Infrastructure, Presentation.Mvc, and Tests. The Domain layer contains the core entities and domain rules, such as training class time validation. The Application layer contains service logic and application-level rules. The Infrastructure layer contains Entity Framework Core, Identity, repositories, database configuration, external authentication setup, and seeders. The Presentation.Mvc project contains the MVC controllers, views, view models, styling, JavaScript, and UI flow. The Tests project contains unit tests and integration tests.

The project uses ASP.NET Core Identity for local authentication and role-based authorization. Normal users are assigned the Member role, while admin functionality is protected with the Admin role. Google and GitHub external login are also configured. New external users are routed through a Set Password flow so they can also use local password login after registering with an external provider.

The database setup is environment-based. In Development, the application uses EF Core InMemory to make local development and testing easier. Outside Development, the application uses SQL Server LocalDB through the DefaultConnection connection string. The application applies EF Core migrations automatically when a relational database provider is used. The database is built with a Code First approach and includes relational constraints such as unique indexes to help prevent duplicate memberships and duplicate bookings.

Several patterns and architectural ideas from the course are used where they fit the project. The solution includes a BaseEntity pattern for shared entity properties, a Domain Exception Pattern for domain rules such as invalid training class times, and a Result Pattern in application services to return clear success or failure results without throwing exceptions for normal validation flow. Repository and Service patterns are used to separate persistence from application logic.

Validation is handled both on the server and in the UI. Important POST actions use ModelState validation and anti-forgery protection. Client-side real-time validation is used for several forms, including authentication forms, account/profile forms, admin class creation, and the customer service form. Error messages are shown clearly in the UI.

The user account area includes profile information, profile image upload with preview, membership information, booked classes, and account deletion. When a profile image is replaced, the old image file is removed. When an account is deleted, related user data is removed and the uploaded profile image file is also deleted.

The training class and booking flow includes rules to only show available future classes, prevent booking classes that have already started, prevent duplicate bookings, require an active membership before booking, and allow users to cancel existing bookings. Admin users can create and delete training classes through the admin area.

Unit and integration tests have been implemented using xUnit. NSubstitute is used in unit tests to isolate application services from repository implementations. Integration tests use EF Core InMemory together with the real DbContext, repositories, and services. The tests cover central domain rules, membership rules, booking rules, training class management, and application/infrastructure integration.

AI was used throughout the project as a support tool and sounding board for structuring ideas, understanding concepts, reviewing requirements, and breaking down more complex problems. It was also used as support when writing and reviewing parts of the code and tests, but the implementation decisions were made based on the assignment requirements and the structure of the project.

Version control has been handled with Git and GitHub, using separate feature branches during development and merging completed parts into main. Commits were made throughout the project to show progression and separate larger parts of the work.


## Running the project locally

Clone the repository and open the solution:

git clone https://github.com/dalebjer88/aspnet-mattias-dalebjer.git
cd aspnet-mattias-dalebjer

Restore and build the solution:

dotnet restore
dotnet build

Run the MVC application:

dotnet run --project CoreFitnessClub.Presentation.Mvc

The application uses the Development environment by default when started from the included development profile. In Development, the database uses EF Core InMemory and is seeded automatically when the application starts.


## Database

Development uses EF Core InMemory:

CoreFitnessClubDb

For non-development environments, the application uses SQL Server LocalDB through the DefaultConnection connection string in CoreFitnessClub.Presentation.Mvc/appsettings.json.

The default LocalDB connection string is:

"DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=CoreFitnessClubDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"

When a relational database provider is used, migrations are applied automatically on application startup.


## External login and secrets

Google and GitHub external login are configured in the application, but real client IDs and client secrets should not be committed to the repository.

For local development, use .NET User Secrets from the MVC project:

cd CoreFitnessClub.Presentation.Mvc

dotnet user-secrets set "Authentication:Google:ClientId" "YOUR_GOOGLE_CLIENT_ID"
dotnet user-secrets set "Authentication:Google:ClientSecret" "YOUR_GOOGLE_CLIENT_SECRET"

dotnet user-secrets set "Authentication:GitHub:ClientId" "YOUR_GITHUB_CLIENT_ID"
dotnet user-secrets set "Authentication:GitHub:ClientSecret" "YOUR_GITHUB_CLIENT_SECRET"

appsettings.Production.json is ignored by Git and should be used only locally if production-specific settings are needed.


## Tailwind CSS

The project uses Tailwind CSS for styling. The generated CSS file is included in the project, but if styling changes are made, install npm packages and rebuild the CSS from the MVC project folder:

cd CoreFitnessClub.Presentation.Mvc
npm install
npm run dev

For a minified production build:

npm run prod


## Tests

Run all tests from the solution root:

dotnet test

The test project includes both unit tests and integration tests.

Unit tests cover central domain and application rules, including:

- Training class time validation
- Training class management service rules
- Membership creation rules
- Booking and cancellation rules

Integration tests use EF Core InMemory and cover:

- Training class repository and service integration
- Membership repository and service integration
- Booking repository and service integration
