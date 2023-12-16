FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

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
COPY ["./CodeSharp.Templates/CodeSharp.Templates.sln", "/TestApp/"]
COPY ["./CodeSharp.Templates/CodeSharp.Playground/CodeSharp.Playground.csproj", "/TestApp/CodeSharp.Playground/"]
COPY ["./CodeSharp.Templates/CodeSharp.Runner/CodeSharp.Runner.csproj", "/TestApp/CodeSharp.Runner/"]
COPY ["./CodeSharp.Templates/CodeSharp.Tests/CodeSharp.Tests.csproj", "/TestApp/CodeSharp.Tests/"]
RUN dotnet restore "/TestApp/CodeSharp.Templates.sln"
COPY ./CodeSharp.Templates ./TestApp

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS final
WORKDIR /test-app
COPY --from=copyAppSample /src/TestApp .
RUN dotnet restore "CodeSharp.Templates.sln"
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 80

#set envs
ENV ASPNETCORE_URLS="http://+:80" \
	ApplicationSettings__SolutionPath="/test-app/CodeSharp.Templates.sln" \
    ApplicationSettings__ConsoleProjectPath="/test-app/CodeSharp.Runner/CodeSharp.Runner.csproj" \
	ApplicationSettings__ConsoleFilePath="/test-app/CodeSharp.Runner/Program.cs" \
	ApplicationSettings__CodeToTestFilePath="/test-app/CodeSharp.Playground/Code.cs" \
	ApplicationSettings__TestProjectPath="/test-app/CodeSharp.Tests/CodeSharp.Tests.csproj" \
	ApplicationSettings__TestFilePath="/test-app/CodeSharp.Tests/Tests.cs" \
	ApplicationSettings__TestReportFilePath="/test-app/CodeSharp.Tests/test-report.xml" \
	ApplicationSettings__ErrorsFilePath="/test-app/code-analysis/errors.txt" \
	ApplicationSettings__CodeAnalysisFilePath="/test-app/code-analysis/warnings.txt" \
	ApplicationSettings__CodeMetricsFilePath="/test-app/code-analysis/metrics.xml"

ENTRYPOINT ["dotnet", "CodeSharp.Executor.dll"]