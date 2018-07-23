FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /app
EXPOSE 51464
EXPOSE 44303

# copy csproj and restore as distinct layers
COPY *.sln .
COPY ChendanKelly/*.csproj ./ChendanKelly/
RUN dotnet restore

# copy everything else and build app
COPY ChendanKelly/. ./ChendanKelly/
WORKDIR /app/ChendanKelly
RUN dotnet publish -c Release -o out

FROM microsoft/dotnet:2.1-aspnetcore-runtime AS runtime
WORKDIR /app
COPY --from=build /app/ChendanKelly/out ./
ENTRYPOINT ["dotnet", "ChendanKelly.dll"]



