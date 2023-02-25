# General information
Current project designed in educational purposes. Performed with such technologies and frameworks as: .NET 6, EF Core, MSSQL database, Razor pages, Automapper, FluentValidation and Swagger support. Designed in microservice architechture.
# Quick Start
1.Clone and open this repository in Visual Studio.<br/>
2.Create empty database in your MSSQL Server.<br/>
3.Replace ConnectionString in IdentityServer->appsettings.json, MeetupAPI->appsettings.json with your connection string.<br/>
4.Run IdentityServer and MeetupAPI and wait untill migrations are completed.<br/>
5.Run WebMvcClient, you'll be redirected to authorization form.<br/>
6.You can authorize via demo credentials: email:demouser@gmail.com, password:demopassword <br/>
or you can register new account.
