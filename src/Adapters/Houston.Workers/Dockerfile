#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
RUN apt-get update && \
    apt-get -qy full-upgrade && \
    apt-get install -qy curl && \
    apt-get install -qy curl && \
    curl -sSL https://get.docker.com/ | sh
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/UserInterface/Houston.Workers/Houston.Workers.csproj", "src/UserInterface/Houston.Workers/"]
RUN dotnet restore "src/UserInterface/Houston.Workers/Houston.Workers.csproj"
COPY . .
WORKDIR "/src/src/UserInterface/Houston.Workers"
RUN dotnet build "Houston.Workers.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Houston.Workers.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Houston.Workers.dll"]
