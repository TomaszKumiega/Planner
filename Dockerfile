FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app

COPY *.sln .
COPY Planner.Model/*.csproj ./Planner.Model/
COPY Planner.API/*.csproj ./Planner.API
RUN dotnet restore Planner.API/Planner.API.csproj

COPY . .
WORKDIR /app/Planner.Model
RUN dotnet build -c Release -o /app

WORKDIR /app/Planner.API
RUN dotnet build Planner.API.csproj -c Release -o /app/build

FROM build AS publish
WORKDIR /app/Planner.API
RUN dotnet publish Planner.API.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Planner.API.dll"]
