FROM mcr.microsoft.com/dotnet/sdk:6.0.410-focal as build

COPY . /source

WORKDIR /source/NotiGest
RUN dotnet restore \
    && dotnet publish --configuration Release -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0.18-focal as runtime

LABEL MAINTAINER Gustavolores15@gmail.com
EXPOSE 5000
ENV ASPNETCORE_URLS="http://0.0.0.0:5000"

COPY --from=build /publish/ /app
WORKDIR /app
ENTRYPOINT [ "dotnet", "NotiGest.dll"]

