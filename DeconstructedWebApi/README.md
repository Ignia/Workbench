# Deconstructed Web API
A stripped-down version of Visual Studio's out-of-the-box Web API 2.x project template to provide bare-bones support for, in particular, ASP.NET Identity web services (e.g., `/Api/Account/Register/`). Does not include any of the MVC, Web API Help, or other web-based devependencies that ship with the out-of-the-box template. Additionally includes an OData controller for exposing other elements from the `Model` project, including `User`, `Post`, and `Comment` end points.

## Changes
While the Deconstructed Web API seeks to maintain parity with the out-of-the-box Web API project template, a number of changes have been made to the templates. These include:
- All files have been reformated and commented, including the use of XmlDocs. 
- Files have been broken down to one file per class; notably, this affects the `/Models` directory. 
- Entity Framework dependencies have been moved to the `Model` project for integration with the data model.
- Web API OData controllers have been added for `Comments` and `Posts` based on the `Model` project.

> *Important:* In the `Startup.Auth.cs` file, this template overrides the `FacebookAuthenticationProvider` class's `OnAuthenticated` property in order to set the `DefaultNameClaimType` to `Email` instead of `Username`, and also adds the `email` property to the `FacebookAuthenticationOptions.Scope` collection. This allows the Web API's external token for Facebook to be used with the Web API's `/Api/Account/RegisterExternal` endpoint (which expects an email address for the username), and `/Api/Account/UserInfo` (which expects that the token's username claim will match that same email address).

## Removed Files
The out-of-the-box Web API template includes a number of dependencies in order to support Web API Help pages. While Web API Help pages are useful, they a) necessitate a much larger footprint for the project, and b) are only compatible with the basic Web API controller (not the Web API OData controller). For these reasons, the following have been removed from the out-of-the-box template:
```
/App_Start
  /BundleConfig.cs
  /FilterConfig.cs
  /RouteConfig.cs
/Areas
  /HelpPages
/Content
/Controllers
  /ValuesController.cs
/Fonts
/Scripts
/Views
/favicon.ico
/Project_Readme.html
```
