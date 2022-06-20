FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Prism.Picshare.Services.Authentication/Prism.Picshare.Services.Authentication.csproj", "Prism.Picshare.Services.Authentication/"]
RUN dotnet restore "Prism.Picshare.Services.Authentication/Prism.Picshare.Services.Authentication.csproj"
COPY . .
WORKDIR "/src/Prism.Picshare.Services.Authentication"
RUN dotnet build "Prism.Picshare.Services.Authentication.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Prism.Picshare.Services.Authentication.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Prism.Picshare.Services.Authentication.dll"]
