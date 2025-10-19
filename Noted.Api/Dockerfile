# Use the official .NET 8 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory
WORKDIR /src

# Copy the solution file
COPY Noted.sln ./

# Copy the project files
COPY Noted.Api/Noted.Api.csproj Noted.Api/
COPY Noted.Application/Noted.Application.csproj Noted.Application/
COPY Noted.Shared/Noted.Shared.csproj Noted.Shared/
COPY Noted.Tests/Noted.Tests.csproj Noted.Tests/


# Restore dependencies
RUN dotnet restore

# Copy the rest of the source code
COPY . .

# Build the application
WORKDIR /src/Noted.Api
RUN dotnet build "Noted.Api.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "Noted.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Create the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy the published application
COPY --from=publish /app/publish .

# Expose the port that the app runs on
EXPOSE 8080

# Set the entry point
ENTRYPOINT ["dotnet", "Noted.Api.dll"]