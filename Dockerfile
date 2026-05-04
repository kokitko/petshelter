FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/PetShelter.Api/PetShelter.Api.csproj", "src/PetShelter.Api/"]
COPY ["src/PetShelter.Application/PetShelter.Application.csproj", "src/PetShelter.Application/"]
COPY ["src/PetShelter.Domain/PetShelter.Domain.csproj", "src/PetShelter.Domain/"]
COPY ["src/PetShelter.Infrastructure/PetShelter.Infrastructure.csproj", "src/PetShelter.Infrastructure/"]
RUN dotnet restore "src/PetShelter.Api/PetShelter.Api.csproj"
COPY . .
RUN dotnet build "src/PetShelter.Api/PetShelter.Api.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "src/PetShelter.Api/PetShelter.Api.csproj" -c Release -o /app/publish
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PetShelter.Api.dll"]