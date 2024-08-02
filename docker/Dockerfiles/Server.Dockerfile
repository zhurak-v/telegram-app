ARG DOTNET_VERSION=8.0
FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS build
WORKDIR /sln

RUN for file in $(ls *.csproj); do mkdir -p ${file%.*}/ && mv $file ${file%.*}/; done

COPY . .

WORKDIR /sln/Essential/Essential.Web/

RUN dotnet restore

RUN dotnet build

RUN dotnet publish -c Release -o /app/publish --no-restore

ARG DOTNET_VERSION=8.0
FROM mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION}

WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Essential.Web.dll"]