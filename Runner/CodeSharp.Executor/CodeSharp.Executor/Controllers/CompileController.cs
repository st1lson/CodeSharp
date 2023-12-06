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
            new CodeExecutor().ExecuteCode(request.Code);
            return Ok("Code executed successfully.");
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
    public void ExecuteCode(string code)
    {
        // Create a temporary directory
        string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempDirectory);

        try
        {
            // Write the code to a temporary file
            string codeFile = Path.Combine(tempDirectory, "DynamicCode.cs");
            File.WriteAllText(codeFile, code);

            // Create a temporary project file
            string projectFile = Path.Combine(tempDirectory, "DynamicProject.csproj");
            File.WriteAllText(projectFile, GetProjectFileContent());

            // Build the project using dotnet build command
            Process process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = $"build {projectFile}",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WorkingDirectory = tempDirectory
                }
            };

            process.Start();
            process.WaitForExit();

            if (process.ExitCode == 0)
            {
                // Run the compiled assembly
                string assemblyFile = Path.Combine(tempDirectory, "bin", "Debug", "net7.0", "DynamicProject.dll");
                Process.Start("dotnet", assemblyFile);
            }
            else
            {
                // Handle build errors
                string errorOutput = process.StandardError.ReadToEnd();
                Console.WriteLine($"Build failed with errors:\n{errorOutput}");
            }
        }
        finally
        {
            // Cleanup: Delete temporary files and directory
            Directory.Delete(tempDirectory, true);
        }
    }

    private static string GetProjectFileContent()
    {
        return @"
<Project Sdk=""Microsoft.NET.Sdk"">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>netcoreapp3.1</TargetFramework>
  </PropertyGroup>
</Project>
";
    }
}