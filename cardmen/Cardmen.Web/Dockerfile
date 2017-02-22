FROM microsoft/aspnetcore:1.1
ARG source
WORKDIR /app
EXPOSE 80
ENV ASPNETCORE_ENVIRONMENT=Development
ENV DOTNET_USE_POLLING_FILE_WATCHER=1
COPY ${source:-obj/Docker/publish} .
