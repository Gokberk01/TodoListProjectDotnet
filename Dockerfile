# Build aşaması
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Çözüm dosyasını kopyala
COPY projectTwo.sln .
COPY todoapi/*.csproj ./todoapi/
COPY todoTest/*.csproj ./todoTest/

# Bağımlılıkları restore et
RUN dotnet restore

# Geri kalan dosyaları kopyala ve build et
COPY todoapi/ ./todoapi/
COPY todoTest/ ./todoTest/
WORKDIR /app/todoapi
RUN dotnet publish -c Release -o /out

# Runtime aşaması
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /out .

# Uygulamayı başlat
ENTRYPOINT ["dotnet", "todoapi.dll"]
