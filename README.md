# Custom Implementation of JWT and Swagger in.NET5

### This is a custom n-tier, loosely coupled backend.NET5 web-api-based application and the main purpose is to sample implementation of basic stuff like authentication and authorization using JWT strategy and Swagger with auth.


## Blogifier
Main start-up Solution that contains middleware and controllers.

### Controllers
Account controllers contain all generic methods that are used to call different operations.

### Middle-Ware
The Start-up class contains all the registration of dependency injection, Context & binding with Services and Repos, implementation of Seed-DB, and Swagger stuff.

## Blogifier.Services
The service project Contains all business logic regarding account methods. Divided into several parts like Interface, implementation, and private methods

## Blogifier.Providers
Providers contain Context, Migrations, and Seed-DB for connecting to the database

## Blogifier.Shared
Shared contains different generic models that are used throughout the application.

## Blogifier.Helpers
Helper contains common contents and security functions.


## Configuration

- Appsettings.json contains the database connection string & sample keys (replace them with yours) for JWT. 
- Open Package Manager Console and write command *add-migration v1 -o Data\Migrations* and hit enter so it will create another migration for database snapshot.
- Run *update-database* to create a database in your SQL Server. Go to SQL Server to make sure is Database created with the name that you have written in your connection string.
- Run the application by choosing *Blogifier* as your startup project.
- Make sure you have an entry in your *Account* DB table with an email *abc@gmail.com*. Password will be encrypted in DB but you can check *Seed-DB class* for decrypted password.
- Make an API hit by going to this URL *https://localhost:5001/swagger/index.html*.
- Try Out the *API/account/sign in* by giving a username and password that are written in *Seed-DB class*. In Response, you will get an access token. 
- On the Swagger index.html page click the Authorize button and add the word *" Bearer PUT you're RECENTLY GENERATED ACCESS TOKEN HERE"*. It will allow you to make an authorized call.
