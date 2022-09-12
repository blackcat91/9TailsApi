FROM mcr.microsoft.com/dotnet/sdk:6.0 
COPY . /app

WORKDIR /app
RUN dotnet publish -c Release -o published
WORKDIR /app/published
EXPOSE 8080
ENV PORT 8080
ENTRYPOINT ["dotnet", "9Tails.dll"]