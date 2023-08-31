#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Adapters/Houston.Workers/Houston.Workers.csproj", "src/Adapters/Houston.Workers/"]
COPY ["src/Core/Houston.Application/Houston.Application.csproj", "src/Core/Houston.Application/"]
COPY ["src/Core/Houston.Core/Houston.Core.csproj", "src/Core/Houston.Core/"]
COPY ["src/Core/Houston.Infrastructure/Houston.Infrastructure.csproj", "src/Core/Houston.Infrastructure/"]
RUN dotnet restore "src/Adapters/Houston.Workers/Houston.Workers.csproj"
COPY . .
WORKDIR "/src/src/Adapters/Houston.Workers"
RUN dotnet build "Houston.Workers.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Houston.Workers.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Houston.Workers.dll"]