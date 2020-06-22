# WebAPI-Angular
This is a simple note-taking app with asp.net core 3.0 web API and angular 8.

The application comes with Identity server open OIDC, pagination, hateoas, vendor specific media type, data shaping, custom validation, etc. 

Server and Client are completely seperated.

Please visit the wiki for more details.

### Project Organization
The solution is organized in three parts, the first one is the server side that consists of a collection of .Net Core Web APIs that access SQLite database through Entity Framework Core. The second part is Identity server 4 as an OpenID-Connect Authentication server. The third part is an Angular 8 single page application that manages all the user interactions and calls the .Net Web APIs.

After getting the code execute the following commands for each project:

SimpleNote.Api : i) update-database ii)dotnet run

SimpleNote.Idp : i)dotnet run

Angular : i) npm install ii) ng serve

Open the web browers with http://localhost:4200/

### Application Walkthrough

![1](https://user-images.githubusercontent.com/16623796/84036031-3cb46080-a9cf-11ea-9a85-348ba62fd57d.png)

![2](https://user-images.githubusercontent.com/16623796/84036132-6077a680-a9cf-11ea-8069-5ebe61f97da0.png)

![3](https://user-images.githubusercontent.com/16623796/84036158-6a99a500-a9cf-11ea-8ab8-b42a718688e3.png)

![4](https://user-images.githubusercontent.com/16623796/84036182-72f1e000-a9cf-11ea-9cbf-a61bc86c6ea6.png)
