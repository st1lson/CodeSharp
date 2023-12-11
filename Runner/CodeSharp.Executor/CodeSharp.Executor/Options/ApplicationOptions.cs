﻿namespace CodeSharp.Executor.Options;

public sealed class ApplicationOptions
{
    internal const string OptionsKey = "ApplicationSettings";

    public required string SolutionPath { get; init; }
    public required string ConsoleProjectPath { get; init; }
    public required string TestProjectPath { get; init; }
    public required string ConsoleFilePath { get; init; }
    public required string TestFilePath { get; init; }
}