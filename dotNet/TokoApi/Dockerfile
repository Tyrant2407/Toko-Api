# Stage 1: Build environment
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build-env
WORKDIR /app

# Copy csproj dan restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy sisa kode dan publish
COPY . ./
# PERHATIKAN BARIS INI: Tidak ada lagi --no-restore
RUN dotnet publish -c Release -o /app/out 

# Stage 2: Runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine

WORKDIR /app
COPY --from=build-env /app/out .

EXPOSE 5174
ENV ASPNETCORE_URLS=http://+:5174

ENTRYPOINT ["dotnet", "TokoApi.dll"]