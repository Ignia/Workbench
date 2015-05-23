# Workbench
The Ignia Workbench is a basic application structure to facilitate testing of new web-related technologies. At its core, the Ignia Workbench provides a common database, web services, and various client implementations to integrate with. The goal is to establish a consistent environment that facilitates testing of frameworks and libraries which depend on the availability of databases, web services, etc., while also providing a comprhenesive enough structure to allow for more nuanced evaluation of features. The latter may include, for instance, one-to-many (1:n) relationship, many-to-many (n:n) relationships, cascading deletes, partial updates, etc.

# Projects
The Ignia Workbench is composed of a series of projects which coordinate with one another to varying degrees. The core of these include:

## Model
Provides a baseline C# data model alongside an Entity Framework 6.x implementation for persisting the model to a SQL database. Additionally the model project implements `IdentityUser` and `IdentityDbContext` so that it can be integrated with ASP.NET Identity. The basic data model includes `User`, `Post`, and `Comment` classes, along with relationships such as `Following`, `Likes`, etc. 

## Web API (`DeconstructedWebApi`)
A stripped-down version of Visual Studio's out-of-the-box Web API 2.x project template to provide bare-bones support for, in particular, ASP.NET Identity web services (e.g., `/Api/Account/Register/`). Does not include any of the MVC, Web API Help, or other web-based devependencies that ship with the out-of-the-box template. Additionally includes an OData controller for exposing other elements from the `Model` project, including `User`, `Post`, and `Comment` end points.

## Client
Exposes the end points from the Web API project. Also offers a location for building a variety of web-based client projects, each to be placed in their own directory (e.g., `/Angular/`). Each client project should seek to expose functionality based on the Web API end points to comprehensively demonstrate how different libraries integrate with these fundamental patterns (e.g., different data relationships, authentication and authorization, etc).

 
