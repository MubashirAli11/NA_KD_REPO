
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Shop.API/Shop.API.csproj", "Shop.API/"]
RUN dotnet restore "Shop.API/Shop.API.csproj"
COPY . .
WORKDIR "/src/Shop.API"
RUN dotnet build "Shop.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Shop.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Shop.API.dll"]