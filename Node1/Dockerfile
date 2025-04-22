# Use .NET 8 ASP.NET base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Use .NET 8 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet build -c Release -o /app/build
RUN dotnet publish -c Release -o /app/publish

# Final stage - using the base image for runtime
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

# IMPORTANT: Make sure your DLL name is correct!
ENTRYPOINT ["dotnet", "Node1.dll"]
