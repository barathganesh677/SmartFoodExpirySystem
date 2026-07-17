FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 80
ENV DOTNET_USE_POLLING_FILE_WATCHER=true
ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src
COPY ["SmartFoodExpirySystem/SmartFoodExpirySystem.csproj", "SmartFoodExpirySystem/"]
RUN dotnet restore "SmartFoodExpirySystem/SmartFoodExpirySystem.csproj"
COPY . .
WORKDIR "/src/SmartFoodExpirySystem"
RUN dotnet build "SmartFoodExpirySystem.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SmartFoodExpirySystem.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SmartFoodExpirySystem.dll"]