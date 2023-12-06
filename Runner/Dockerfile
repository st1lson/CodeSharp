FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

#build Web API
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["./CodeSharp.Executor/CodeSharp.Executor/CodeSharp.Executor.csproj", "WebAPI/CodeSharp.Executor/"]
RUN dotnet restore "./WebAPI/CodeSharp.Executor/CodeSharp.Executor.csproj"
COPY ./CodeSharp.Executor ./WebAPI

WORKDIR "/src/WebAPI/CodeSharp.Executor"
RUN dotnet build "./CodeSharp.Executor.csproj" -c Release -o /app/build

#publish Web API
FROM build AS publish
RUN dotnet publish "./CodeSharp.Executor.csproj" -c Release -o /app/publish /p:UseAppHost=false

#copy app sample
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS copyAppSample
WORKDIR /src
COPY ["./AppSample/AppSample.sln", "/TestApp/"]
COPY ["./AppSample/AppSample/AppSample.csproj", "/TestApp/AppSample/"]
COPY ["./AppSample/AppTests/AppTests.csproj", "/TestApp/AppTests/"]
RUN dotnet restore "/TestApp/AppSample.sln"
COPY ./AppSample ./TestApp

FROM base AS final
WORKDIR /test-app
COPY --from=copyAppSample /src .
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CodeSharp.Executor.dll"]