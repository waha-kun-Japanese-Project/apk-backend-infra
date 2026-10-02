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

# CommunityService.csproj references CommanLib and several sibling projects
# under CommuntiyService/ (Core/, Infrastructure/, Clients/, Communtiy.Shared/)
# - copying the whole context before restore (rather than just this one
# csproj) avoids "project file not found" restore failures from those
# cross-folder ProjectReferences, same reasoning as ReportService's Dockerfile.
COPY . .
WORKDIR "/src/CommuntiyService/CommunityService"
RUN dotnet restore "./CommunityService.csproj"
RUN dotnet build "./CommunityService.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CommunityService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CommunityService.dll"]