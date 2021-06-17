#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["AuditLog.Data.MySql/AuditLog.Data.MySql.csproj", "AuditLog.Data.MySql/"]
COPY ["AuditLog.API/AuditLog.API.csproj", "AuditLog.API/"]
RUN dotnet restore "AuditLog.API/AuditLog.API.csproj"
COPY . .
WORKDIR "/src/AuditLog.API"
RUN dotnet build "AuditLog.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AuditLog.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuditLog.API.dll"]