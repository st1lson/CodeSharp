using System.Diagnostics;
using Carter;
using CodeSharp.Executor.Contracts;
using MediatR;

namespace CodeSharp.Executor.Features.Compilation;

public static class CompileCode
{
    public record Command : IRequest<CompilationResponse>
    {
        public required string Code { get; init; }
    }
    
    public sealed class Handler : IRequestHandler<Command, CompilationResponse>
    {
        public Task<CompilationResponse> Handle(Command request, CancellationToken cancellationToken)
        {
            WriteCodeToFile(request.Code);
            return Task.FromResult(CompileCode());
        }
        
        private static void WriteCodeToFile(string code)
        {
            try
            {
                File.WriteAllText(@"M:\study\CodeSharp\Runner\AppSample\AppSample\Program.cs", code);
                Console.WriteLine($@"Code written to file: M:\study\CodeSharp\Runner\AppSample\AppSample\Program.cs");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing code to file: {ex.Message}");
            }
        }
        
        private static CompilationResponse CompileCode()
        {
            var result = new CompilationResponse();
            
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            
            try
            {
                using Process process = new Process();
                process.StartInfo.FileName = "dotnet";
                process.StartInfo.Arguments = "build M:\\study\\CodeSharp\\Runner\\AppSample\\AppSample.sln";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;

                StringWriter outputWriter = new StringWriter();
                StringWriter errorWriter = new StringWriter();

                process.OutputDataReceived += (_, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        outputWriter.WriteLine($"[Output] {e.Data}");
                    }
                };

                process.ErrorDataReceived += (_, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        errorWriter.WriteLine($"[Error] {e.Data}");
                    }
                };

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();
                    
                stopwatch.Stop();
                result.TimeTaken = stopwatch.Elapsed;

                int exitCode = process.ExitCode;

                result.Success = (exitCode == 0);
                result.Output = outputWriter.ToString();
                result.Error = errorWriter.ToString();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Error = $"An error occurred: {ex.Message}";
            }

            return result;
        }
    }
}

public class CompileCodeEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/compiler/code", async (CompilationRequest request, ISender sender) =>
        {
            var command = new CompileCode.Command
            {
                Code = request.Code
            };

            var result = await sender.Send(command);

            return Results.Ok(result);
        });
    }
}