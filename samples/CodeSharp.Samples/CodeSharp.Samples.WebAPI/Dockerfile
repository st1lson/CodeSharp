#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CodeSharp.Samples.WebAPI/CodeSharp.Samples.WebAPI.csproj", "CodeSharp.Samples.WebAPI/"]
RUN dotnet restore "./CodeSharp.Samples.WebAPI/./CodeSharp.Samples.WebAPI.csproj"
COPY . .
WORKDIR "/src/CodeSharp.Samples.WebAPI"
RUN dotnet build "./CodeSharp.Samples.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CodeSharp.Samples.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CodeSharp.Samples.WebAPI.dll"]