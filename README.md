# Custom Implementation of JWT and Swagger

### This is custom n-tier, loose coupled backend web-api based application and the main purpose is to sample implementation of basic stuff like authtication and authorization using JWT strategy and Swagger with auth.


## Blogifier
Main start up Solution that contains middle ware and controllers.

### Controllers
Account controller contain all generic methods that are using to calling different operations.

### Middle-Ware
Start-up class contains all the registration of dependency injection, Context & binding with Services and Repos, implementaion of Seed-DB and Swagger stuff.

## Blogifier.Services
Service project Contains the all business logics regarding account methods. Divided in several parts like Interface, Implementaion and private methods

## Blogifier.Providers
Providers contains Context, Migrations and Seed-DB for connecting to database

## Blogifier.Shared
Shared contains different generic models that are using through out application.

## Blogifier.Helpers
Helper contains common contants and security fucntions.


## Configuration

- Appsettings.json contains the database connection string & sample keys (replace them with yours) for JWT. 
- Open Package Manger Console and write command *add-migration v1 -o Data\Migrations* and hit enter so it will create another migration for databae snapshot.
- Run *update-database* to create database in your SQL Server. Go to SQL Server to make sure is Database created with the name that you have written in your connection string.
- Run the application by choosing *Blogifier* as your startup project.
- Make sure you have entry in your *Account* db table with an email *abc@gmail.com*. Password will be encrypted in db but you can check *Seed-db class* for decrypted password.
- Make an API hit by going to this url *https://localhost:5001/swagger/index.html*.
-   
