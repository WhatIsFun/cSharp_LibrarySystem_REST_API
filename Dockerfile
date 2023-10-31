
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5232
EXPOSE 7062

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["cSharp_LibrarySystemWebAPI.csproj", "."]
RUN dotnet restore "./cSharp_LibrarySystemWebAPI.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "cSharp_LibrarySystemWebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "cSharp_LibrarySystemWebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "cSharp_LibrarySystemWebAPI.dll"]