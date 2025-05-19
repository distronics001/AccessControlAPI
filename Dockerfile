# Étape 1 : Base runtime .NET 8
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Étape 2 : SDK pour compiler l'application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copie tous les fichiers du projet
COPY . .

# Restaure les packages
RUN dotnet restore "./AccessControlAPI.csproj"

# Compile et publie en mode Release
RUN dotnet publish "./AccessControlAPI.csproj" -c Release -o /app/publish

# Étape 3 : Création de l'image finale
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

# Point d’entrée de l'application
ENTRYPOINT ["dotnet", "AccessControlAPI.dll"]
