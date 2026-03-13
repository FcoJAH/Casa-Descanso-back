# Etapa de compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar el proyecto y restaurar dependencias
# AJUSTE: Si tu .csproj tiene otro nombre, cámbialo aquí
COPY ["CasaDescanso.Api/CasaDescanso.Api.csproj", "CasaDescanso.Api/"]
RUN dotnet restore "CasaDescanso.Api/CasaDescanso.Api.csproj"

# Copiar todo el código y publicar
COPY . .
WORKDIR "/src/CasaDescanso.Api"
RUN dotnet publish -c Release -o /app/publish

# Etapa final (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Exponer el puerto que usa Render
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "CasaDescanso.Api.dll"]