FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src
COPY ["src/UserCrud.Web.Host/UserCrud.Web.Host.csproj", "src/UserCrud.Web.Host/"]
COPY ["src/UserCrud.Web.Core/UserCrud.Web.Core.csproj", "src/UserCrud.Web.Core/"]
COPY ["src/UserCrud.Application/UserCrud.Application.csproj", "src/UserCrud.Application/"]
COPY ["src/UserCrud.Core/UserCrud.Core.csproj", "src/UserCrud.Core/"]
COPY ["src/UserCrud.EntityFrameworkCore/UserCrud.EntityFrameworkCore.csproj", "src/UserCrud.EntityFrameworkCore/"]
WORKDIR "/src/src/UserCrud.Web.Host"
RUN dotnet restore 

WORKDIR /src
COPY ["src/UserCrud.Web.Host", "src/UserCrud.Web.Host"]
COPY ["src/UserCrud.Web.Core", "src/UserCrud.Web.Core"]
COPY ["src/UserCrud.Application", "src/UserCrud.Application"]
COPY ["src/UserCrud.Core", "src/UserCrud.Core"]
COPY ["src/UserCrud.EntityFrameworkCore", "src/UserCrud.EntityFrameworkCore"]
WORKDIR "/src/src/UserCrud.Web.Host"
RUN dotnet publish -c Release -o /publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
EXPOSE 80
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "UserCrud.Web.Host.dll"]
