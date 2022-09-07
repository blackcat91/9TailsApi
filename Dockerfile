FROM mcr.microsoft.com/dotnet/sdk:6.0 
COPY ./9Tails /app/9Tails
COPY ./DataAccess /app/DataAccess
WORKDIR /app/9Tails
RUN dotnet dev-certs https
RUN dotnet dev-certs https --trust
RUN dotnet publish "9Tails.csproj"
ENV PORT 8080
ENTRYPOINT dotnet run "9Tails.csproj"