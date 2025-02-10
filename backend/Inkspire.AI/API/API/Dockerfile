FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

COPY ../../Inkspire.AI.sln ./
COPY ../../Core/Core/Core.csproj Core/Core/
COPY ../../API/API/API.csproj API/API/

RUN dotnet restore Inkspire.AI.sln

COPY ../../ ./
WORKDIR /app/API/API
RUN dotnet publish API.csproj -c Release -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /publish ./

ENTRYPOINT ["dotnet", "API.dll"]

EXPOSE 8080
