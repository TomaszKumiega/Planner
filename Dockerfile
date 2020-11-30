FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base	
WORKDIR /app	
EXPOSE 80	
EXPOSE 443	

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY Planner.API/Planner.API.csproj Planner.API/
COPY Planner.Model/Planner.Model.csproj Planner.Model/
RUN dotnet restore Planner.API/Planner.API.csproj

COPY . .
WORKDIR /src/Planner.Model
RUN dotnet build Planner.Model.csproj -c Release -o /app/build

WORKDIR /src/Planner.API
RUN dotnet build Planner.API.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish Planner.API.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Planner.API.dll"]
