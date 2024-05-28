FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/PowerDiary.Web/PowerDiary.Web.csproj", "src/PowerDiary.Web/"]
COPY ["src/PowerDiary.Domain/PowerDiary.Domain.csproj", "src/PowerDiary.Domain/"]
COPY ["src/PowerDiary.Data/PowerDiary.Data.csproj", "src/PowerDiary.Data/"]

ENV DOTNET_EnableWriteXorExecute=0
RUN dotnet nuget locals all --clear
RUN dotnet restore "src/PowerDiary.Web/PowerDiary.Web.csproj"

COPY . .
WORKDIR "/src/src/PowerDiary.Web"
RUN dotnet publish "PowerDiary.Web.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "PowerDiary.Web.dll"]
