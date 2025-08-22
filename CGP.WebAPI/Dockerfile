# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy .csproj từng layer
COPY ["CGP.WebAPI/CGP.WebAPI.csproj", "CGP.WebAPI/"]
COPY ["CGP.Application/CGP.Application.csproj", "CGP.Application/"]
COPY ["CGP.Contract/CGP.Contract.csproj", "CGP.Contract/"]
COPY ["CGP.Domain/CGP.Domain.csproj", "CGP.Domain/"]
COPY ["CGP.Infrastructure/CGP.Infrastructure.csproj", "CGP.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "CGP.WebAPI/CGP.WebAPI.csproj"

# Copy toàn bộ source
COPY . .

# Build
WORKDIR "/src/CGP.WebAPI"
RUN dotnet build -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CGP.WebAPI.dll"]
