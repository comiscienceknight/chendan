FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-1709 AS base
WORKDIR /app
EXPOSE 51464
EXPOSE 44303

FROM microsoft/dotnet:2.1-sdk-nanoserver-1709 AS build
WORKDIR /src
COPY ChendanKelly/ChendanKelly.csproj ChendanKelly/
RUN dotnet restore ChendanKelly/ChendanKelly.csproj
COPY . .
WORKDIR /src/ChendanKelly
RUN dotnet build ChendanKelly.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish ChendanKelly.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ChendanKelly.dll"]
