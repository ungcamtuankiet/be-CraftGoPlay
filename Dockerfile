# Base image with only the .NET runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build stage with the .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Install EF Core tools
RUN dotnet tool install --global dotnet-ef

# Ensure the dotnet tools path is available
ENV PATH="$PATH:/root/.dotnet/tools"

# Copy and restore dependencies
COPY ["CGP.WebAPI/CGP.WebAPI.csproj", "CGP.WebAPI/"]
COPY ["CGP.Application/CGP.Application.csproj", "CGP.Application/"]
COPY ["CGP.Contract/CGP.Contract.csproj", "CGP.Contract/"]
COPY ["CGP.Domain/CGP.Domain.csproj", "CGP.Domain/"]
COPY ["CGP.Infrastructure/CGP.Infrastructure.csproj", "CGP.Infrastructure/"]
RUN dotnet restore "./CGP.WebAPI/CGP.WebAPI.csproj"

COPY . .
WORKDIR "/src/CGP.WebAPI"
RUN dotnet build "./CGP.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CGP.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime stage (no SDK, only runtime)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Ensure the EF tools are still available (optional, for debugging)
ENV PATH="$PATH:/root/.dotnet/tools"

ENTRYPOINT ["dotnet", "CGP.WebAPI.dll"]