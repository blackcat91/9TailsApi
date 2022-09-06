FROM mcr.microsoft.com/dotnet/sdk:6.0
RUN mkdir /app/9Tails
RUN mkdir /app/DataAccess

COPY . /app/9Tails
COPY ../DataAccess /app/DataAccess
WORKDIR /app/9Tails
RUN dotnet publish
ENTRYPOINT dotnet run