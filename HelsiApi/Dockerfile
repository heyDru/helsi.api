FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY HelsiApi/HelsiApi.csproj HelsiApi/
COPY Data/Data/Data.csproj Data/Data/
COPY Common.Enums/Common.Enums.csproj Common.Enums/
COPY Services/Services.csproj Services/
COPY Common.Models/Common.Models.csproj Common.Models/
COPY Common.Extensions/Common.Extensions.csproj Common.Extensions/
RUN dotnet restore HelsiApi/HelsiApi.csproj
COPY . .
WORKDIR /src/HelsiApi
RUN dotnet build HelsiApi.csproj -c Development -o /app

FROM build AS publish
RUN dotnet publish HelsiApi.csproj -c Development -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "HelsiApi.dll"]
