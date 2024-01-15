# ArtworkProject API 
Repository for the API backend for Artwork Project.

## Setup
To run this backend locally on your machine you need to install:
- .NET 8
- PostgreSQL

Create a new database in Postgresql named ArtworkProject, and keep track of the login username and password and the database port (default: 5432).

Clone the repository and open it in your favourite IDE (Visual Studio). We need to set the database connection string. 
In VS this is done by right-clicking on Connected Services in the API project and selecting Manage connected services, then click the ... next to Secrets.json (Local) and Manage user secrets. 
If you dont have Secrets.json you can create this by doing Add new service dependency in Service Dependencies.
The connection string should be on the form:
```
{
  "ConnectionStrings:ConnectionString": "Server=localhost;Database=ArtworkProject;Port={PORTNUMBER};User Id={USERID};Password={PASSWORD};Ssl Mode=Prefer; Trust Server Certificate=true"
}
```
Where {PORTNUMBER} is the port you set in PostgreSQL (default: 5432), {USERID} is your postgres username (default: postgres) and {PASSWORD} is the password you set for your database.

If you try to run the project now you will get errors about missing dependencies. 
To fix this go to Tools -> NuGet Package Manager -> Package Manager Settings then click on Package Sources on the left. 
Press [+] and add `Name: nuget.org` and `Source: https://api.nuget.org/v3/index.json` and click ok. 
If you now try to run the program via the ▶️ArtworkProject button on the top toolbar it will install all required dependencies then launch. 

Launching the program will by default open an autogenerated Swagger page at https://localhost:7280/swagger/index.html which in some browsers is flagged as potentially unsafe (at least Firefox), in which case you will have to allow loading the page in your browser in order to use the Swagger ui to test the api (note that the API is still running even if you dont do this, so you can use it via other tools). 

The database is still not properly initialized, but now we are ready to perform the database migrations. 
Open the NuGet Package Manager Console by going to Tools -> NuGet Package Manager -> NuGet Package Manager Console.
Input the following command:
```
dotnet tool install --global dotnet-ef
```
There are two ways to perform the database migration.

The recommended way is to do the following:
- In the Package Manager Console change the Default project dropdown to Database.
- Input the following commands:
- `Update-Database`

The alternative way if you are not using VS and the NuGet console is to do the following:
`dotnet ef database update --project Database --startup-project API`

You should now be ready to run the project.