FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app

COPY *.sln .
COPY ./Planner.API/ ./Planner.API
RUN dotnet restore

COPY Planner.API/. ./Planner.API
WORKDIR /app/Planner.API
RUN dotnet build

FROM build AS publish
WORKDIR /app/Planner.API
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=publish /app/Planner.API/out ./
ENTRYPOINT ["dotnet", "Planner.API.dll"]
