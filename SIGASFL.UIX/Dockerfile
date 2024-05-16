#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SIGASFL.UIX/SIGASFL.UIX.csproj", "SIGASFL.UIX/"]
COPY ["SIGASFL.Models/SIGASFL.Models.csproj", "SIGASFL.Models/"]
COPY ["SIGASFL.Services/SIGASFL.Services.csproj", "SIGASFL.Services/"]
COPY ["SIGASFL.Helpers/SIGASFL.Helpers.csproj", "SIGASFL.Helpers/"]
COPY ["SIGASFL.Repositories/SIGASFL.Repositories.csproj", "SIGASFL.Repositories/"]
COPY ["SIGASFL.Entities/SIGASFL.Entities.csproj", "SIGASFL.Entities/"]
RUN dotnet restore "SIGASFL.UIX/SIGASFL.UIX.csproj"
COPY . .
WORKDIR "/src/SIGASFL.UIX"
RUN dotnet build "SIGASFL.UIX.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SIGASFL.UIX.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SIGASFL.UIX.dll"]