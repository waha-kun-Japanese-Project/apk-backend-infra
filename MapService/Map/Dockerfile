# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Map.csproj -> Map.Presentation / Map.Persistence / Map.Service -> Map.Domain, Map.Shared,
# Map.ServiceAbsraction and CommanLib (repo root). MapService has NO compile-time
# dependency on IssueService; it only reads IssueDb through its own IssueDbContext.
# The full context is copied before restore because of the cross-folder CommanLib reference.
COPY . .
WORKDIR "/src/MapService/Map"
RUN dotnet restore "./Map.csproj"
RUN dotnet build "./Map.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Map.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Map.dll"]
