# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5000

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/CodeSphere/CodeSphere.csproj", "src/CodeSphere/"]
COPY ["src/CodeSphere.Application/CodeSphere.Application.csproj", "src/CodeSphere.Application/"]
COPY ["src/CodeSphere.DataAccess/CodeSphere.DataAccess.csproj", "src/CodeSphere.DataAccess/"]
COPY ["src/CodeSphere.Domain/CodeSphere.Domain.csproj", "src/CodeSphere.Domain/"]
RUN dotnet restore "./src/CodeSphere/CodeSphere.csproj"
COPY . .
WORKDIR "/src/src/CodeSphere"
RUN dotnet build "./CodeSphere.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CodeSphere.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CodeSphere.dll"]
