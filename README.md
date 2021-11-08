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


