# Base runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
# Render yêu cầu container phải lắng nghe cổng $PORT
ENV ASPNETCORE_URLS=http://+:${PORT}
EXPOSE 10000

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["CGP.WebAPI/CGP.WebAPI.csproj", "CGP.WebAPI/"]
COPY ["CGP.Application/CGP.Application.csproj", "CGP.Application/"]
COPY ["CGP.Contract/CGP.Contract.csproj", "CGP.Contract/"]
COPY ["CGP.Domain/CGP.Domain.csproj", "CGP.Domain/"]
COPY ["CGP.Infrastructure/CGP.Infrastructure.csproj", "CGP.Infrastructure/"]
RUN dotnet restore "CGP.WebAPI/CGP.WebAPI.csproj"
COPY . .
WORKDIR "/src/CGP.WebAPI"
RUN dotnet publish "CGP.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CGP.WebAPI.dll"]
