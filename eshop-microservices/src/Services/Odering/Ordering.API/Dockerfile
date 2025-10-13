# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
ENV ASPNETCORE_URLS=http://+:8080;https://+:8081

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/Odering/Ordering.API/Ordering.API.csproj", "src/Services/Odering/Ordering.API/"]
COPY ["src/Services/Odering/Odering.Infrastructure/Odering.Infrastructure.csproj", "src/Services/Odering/Odering.Infrastructure/"]
COPY ["src/Services/Odering/Ordering.Application/Ordering.Application.csproj", "src/Services/Odering/Ordering.Application/"]
COPY ["src/BuildingBlocks.Messaging/BuildlingBlocks.Messaging/BuildlingBlocks.Messaging.csproj", "src/BuildingBlocks.Messaging/BuildlingBlocks.Messaging/"]
COPY ["src/Services/Odering/Ordering.Domain/Ordering.Domain.csproj", "src/Services/Odering/Ordering.Domain/"]
RUN dotnet restore "./src/Services/Odering/Ordering.API/Ordering.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Odering/Ordering.API"
RUN dotnet build "./Ordering.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Ordering.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ordering.API.dll"]