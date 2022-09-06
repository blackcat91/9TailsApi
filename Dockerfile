FROM mcr.microsoft.com/dotnet/sdk:6.0

COPY ./9Tails /app/9Tails
COPY ./DataAccess /app/DataAccess
WORKDIR /app/9Tails
RUN dir
RUN dotnet publish "9Tails.csproj"
ENTRYPOINT dotnet run "9Tails.csproj"