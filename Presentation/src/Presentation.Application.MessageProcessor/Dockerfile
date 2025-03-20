# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0  AS base
USER $APP_UID
WORKDIR /app


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["Presentation/src/Presentation.Application.MessageProcessor/Presentation.Application.MessageProcessor.csproj", "Presentation/src/Presentation.Application.MessageProcessor/"]
COPY ["Common/src/Common.Infrastructure/Common.Infrastructure.csproj", "Common/src/Common.Infrastructure/"]
COPY ["Common/src/Common.LanguageExtensions/Common.LanguageExtensions.csproj", "Common/src/Common.LanguageExtensions/"]
COPY ["Presentation/src/Presentation.Core.DataModels/Presentation.Core.DataModels.csproj", "Presentation/src/Presentation.Core.DataModels/"]
COPY ["Common/src/Common.Core.Boundary/Common.Core.Boundary.csproj", "Common/src/Common.Core.Boundary/"]
COPY ["Presentation/src/Presentation.Core/Presentation.Core.csproj", "Presentation/src/Presentation.Core/"]
COPY ["Monsters/src/Monsters.Core.Boundary/Monsters.Core.Boundary.csproj", "Monsters/src/Monsters.Core.Boundary/"]
COPY ["Presentation/src/Presentation.Persistence/Presentation.Persistence.csproj", "Presentation/src/Presentation.Persistence/"]
RUN dotnet restore "./Presentation/src/Presentation.Application.MessageProcessor/Presentation.Application.MessageProcessor.csproj"
COPY . .
WORKDIR "/src/Presentation/src/Presentation.Application.MessageProcessor"
RUN dotnet build "./Presentation.Application.MessageProcessor.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Presentation.Application.MessageProcessor.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Presentation.Application.MessageProcessor.dll"]