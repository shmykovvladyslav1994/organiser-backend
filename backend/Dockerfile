# build stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o out

# runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "backend.dll"]