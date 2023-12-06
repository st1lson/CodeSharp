using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CodeSharp.Executor.Controllers;

[Route("api/[controller]")]
public class CompileController : ControllerBase
{
    [HttpPost]
    public IActionResult Compile([FromBody] Request request)
    {
        try
        {
            var codeExecutor = new CodeExecutor();
            codeExecutor.WriteCodeToFile(request.Code);
            return Ok(codeExecutor.CompileCode());
        }
        catch (Exception ex)
        {
            return BadRequest($"Error executing code: {ex.Message}");
        }
    }
}

public class Request
{
    public string Code { get; set; }
}

public class CodeExecutor
{
    public void WriteCodeToFile(string code)
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

    public class CompilationResult
    {
        public bool Success { get; set; }
        public string Output { get; set; }
        public string Error { get; set; }
        public TimeSpan TimeTaken { get; set; }
    }

    public CompilationResult CompileCode()
    {
        var result = new CompilationResult();
        
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        
        try
        {
            using (Process process = new Process())
            {
                process.StartInfo.FileName = "dotnet";
                process.StartInfo.Arguments = "build M:\\study\\CodeSharp\\Runner\\AppSample\\AppSample.sln";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;

                StringWriter outputWriter = new StringWriter();
                StringWriter errorWriter = new StringWriter();

                process.OutputDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        outputWriter.WriteLine($"[Output] {e.Data}");
                    }
                };

                process.ErrorDataReceived += (sender, e) =>
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
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.Error = $"An error occurred: {ex.Message}";
        }

        return result;
    }
}