FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 7071

ENV ASPNETCORE_URLS=http://+:7071

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WebApiCleanArchitecture/WebApiCleanArchitecture.csproj", "WebApiCleanArchitecture/"]
COPY ["Persistence/Persistence.csproj", "Persistence/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Identity/Identity.csproj", "Identity/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Application/Application.csproj", "Application/"]
RUN dotnet restore "WebApiCleanArchitecture/WebApiCleanArchitecture.csproj"
COPY . .
WORKDIR "/src/WebApiCleanArchitecture"
RUN dotnet build "WebApiCleanArchitecture.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApiCleanArchitecture.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApiCleanArchitecture.dll"]
