FROM mcr.microsoft.com/dotnet/sdk:8.0.101-jammy-amd64 as build

COPY . /source

WORKDIR /source/NotiGest
RUN dotnet restore \
    && dotnet publish --configuration Release -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0.1-jammy-amd64 as runtime

LABEL MAINTAINER Gustavolores15@gmail.com
EXPOSE 5000
ENV ASPNETCORE_URLS="http://0.0.0.0:5000"

COPY --from=build /publish/ /app
WORKDIR /app
ENTRYPOINT [ "dotnet", "NotiGest.dll"]

