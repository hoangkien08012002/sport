# Base image for the runtime
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=https://+:443;http://+:80

# Build image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy and restore project files
COPY ["HightSportShopWebAPI/HightSportShopWebAPI.csproj", "HightSportShopWebAPI/"]
COPY ["HightSportShopDataAccess/HightSportShopDataAccess.csproj", "HightSportShopDataAccess.Core/"]
COPY ["HightSportShopBusiness/HightSportShopBusiness.csproj", "HightSportShopBusiness/"]
RUN dotnet restore "HightSportShopWebAPI/HightSportShopWebAPI.csproj"

# Copy the rest of the source code and build
COPY . .
WORKDIR "/src/HightSportShopWebAPI"
ARG BUILD_CONFIGURATION=Release
RUN dotnet build "HightSportShopWebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "HightSportShopWebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage - runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HightSportShopWebAPI.dll"]
