FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

ARG NUGET_AUTH_TOKEN

WORKDIR /source

# copy files and restore
COPY . .
RUN bash -c 'dotnet nuget add source --username prism-be --password ${NUGET_AUTH_TOKEN} --store-password-in-clear-text --name github "https://nuget.pkg.github.com/prism-be/index.json"'
RUN dotnet restore

# copy and publish app and libraries
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine
EXPOSE 80
ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Prism.Picshare.Services.Authentication.dll"]