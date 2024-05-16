Ecosystem

1. Visual Studio 2022
2. .NET 6.0
	Front End - UIX Blazor Web Assembly
	- Authentication using Custom Implementation ASP.NET Roles
	- API Auth using JWToken
	Backend - .NET CORE

Clean Architecture (Onion Architecture based)

00. - Shared Layer
	SIGASFL.Helpers

01. - Domain Layer
	SIGASFL.Entities
	SIGASFL.Models

02. - Repository Layer
	SIGASFL.Repositories

03. - Service Layer
	SIGASFL.Service

04. - Presentation Layer
	4.1 - WEB
		SIGASFL.UIX = Blazor Project using .Net 6.0
	4.2 - API
		SIGASFL.Restful => Net Core WebAPI Project using .Net 6.0

05. - Unit Testing Layer
	SIGASFL.UnitTest
