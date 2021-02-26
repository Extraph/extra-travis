# #See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Ema.Ijoins.Api.csproj", ""]
RUN dotnet restore "Ema.Ijoins.Api.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "Ema.Ijoins.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ema.Ijoins.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ema.Ijoins.Api.dll"]
