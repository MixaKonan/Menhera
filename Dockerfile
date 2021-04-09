FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Menhera.csproj", ""]
RUN dotnet restore "./Menhera.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Menhera.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Menhera.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Menhera.dll"]