# Use a specific base image that includes the self-signed certificate
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
RUN apt-get update && apt-get install -y telnet net-tools nano curl jq && \
    rm -rf /var/lib/apt/lists/*
WORKDIR /app
EXPOSE 80
EXPOSE 443

RUN sed -i '/\[openssl_init\]/a ssl_conf = ssl_sect' /etc/ssl/openssl.cnf
RUN printf "\n[ssl_sect]\nsystem_default = system_default_sect\n" >> /etc/ssl/openssl.cnf
RUN printf "\n[system_default_sect]\nMinProtocol = TLSv1.2\nCipherString = DEFAULT@SECLEVEL=0" >> /etc/ssl/openssl.cnf

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["api/src/Promomash.Host/Promomash.Host.csproj", "Promomash/src/Promomash.Host/"]
COPY ["api/src/Promomash.Infrastructure/Promomash.Infrastructure.csproj", "Promomash/src/Promomash.Infrastructure/"]
COPY ["api/src/Promomash.Domain/Promomash.Domain.csproj", "Promomash/src/Promomash.Domain/"]
COPY ["api/src/Promomash.EntityFramework/Promomash.EntityFramework.csproj", "Promomash/src/Promomash.EntityFramework/"]
RUN dotnet restore "api/src/Promomash.Host/Promomash.Host.csproj"
COPY . .
WORKDIR "Promomash/src/Promomash.Host"
RUN dotnet build "Promomash.Host.csproj" -c Release -o /app/build

FROM build AS publish

RUN dotnet publish "Promomash.Host.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Promomash.Host.dll"]
