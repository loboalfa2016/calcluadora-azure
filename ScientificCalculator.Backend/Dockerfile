# Stage 1: Runtime Base (.NET 10)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_HTTP_PORTS=8080

# Stage 2: SDK Build (.NET 10)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["ScientificCalculator.Backend.csproj", "./"]
RUN dotnet restore "./ScientificCalculator.Backend.csproj"
COPY . .
RUN dotnet build "ScientificCalculator.Backend.csproj" -c Release -o /app/build

# Stage 3: Publish
FROM build AS publish
RUN dotnet publish "ScientificCalculator.Backend.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 4: Final Image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ScientificCalculator.Backend.dll"]
