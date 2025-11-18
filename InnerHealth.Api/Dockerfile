# ================================
# STAGE 1 — BUILD
# ================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia tudo
COPY . .

# Restaura dependências
RUN dotnet restore

# Publica em modo Release
RUN dotnet publish -c Release -o /out

# ================================
# STAGE 2 — RUNTIME
# ================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copia arquivos publicados
COPY --from=build /out .

# Porta exposta pela API
EXPOSE 8080

# Variável de ambiente opcional
ENV ASPNETCORE_URLS=http://+:8080

# Inicia a aplicação
ENTRYPOINT ["dotnet", "InnerHealth.Api.dll"]
