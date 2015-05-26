# Workbench
The Ignia Workbench is a basic application structure to facilitate testing of web-related technologies. At its core, the Ignia Workbench provides a common database, web services, and various client implementations to integrate with. The goal is to establish a consistent environment that facilitates testing of frameworks and libraries which depend on the availability of databases, web services, etc., while also providing a comprhenesive enough structure to allow for more nuanced evaluation of their features. The latter may include, for instance, one-to-many (1:n) relationship, many-to-many (n:n) relationships, cascading deletes, partial updates, etc.

# Projects
The Ignia Workbench is composed of a series of projects which coordinate with one another. The core of these include:

## [Model](./Model/)
A baseline C# data model alongside an Entity Framework 6.x implementation for persisting the model to a SQL database. Additionally the model project implements `IdentityUser` and `IdentityDbContext` so that it can be integrated with ASP.NET Identity. The basic data model includes `User`, `Post`, and `Comment` classes, along with relationships such as `Following`, `Likes`, etc. 

## [Web API](./WebApi/)
A stripped-down version of Visual Studio's out-of-the-box Web API 2.x project template to provide bare-bones support for, in particular, ASP.NET Identity web services (e.g., `/Api/Account/Register/`). Does not include any of the MVC, Web API Help, or other web-based devependencies that ship with the out-of-the-box template. Additionally includes an OData controller for exposing other elements from the `Model` project, including `User`, `Post`, and `Comment` end points.

## [Client](./Client/)
Offers a location for building a variety of web-based client projects, each to be placed in their own directory (e.g., `/Angular/`). Exposes the end points from the Web API project so they're available for integration; each client project should seek to map to this functionality as a means of comprehensively demonstrate these fundamental patterns (e.g., client-server interation, various data relationships, authentication and authorization, etc).

 
