using System.CommandLine;
using System.CommandLine.Parsing;
using Crumpet.Interpreter;

namespace Crumpet.Console;

public static class Program
{
    public static async Task<int> Main(string[] args)
    {
        Argument<FileInfo> fileArg = new Argument<FileInfo>("file", description: "The file to execute");
        fileArg.AddValidator(ValidateCrumpetFile);
        Option<string> entryPointName = new Option<string>("--entry", () => "main", "Entry point name");
        Argument<string[]> programArgs = new Argument<string[]>();
        RootCommand rootCommand = new RootCommand()
        {
            fileArg,
            entryPointName,
            programArgs
        };
        rootCommand.Add(fileArg);
        rootCommand.SetHandler(Entry, fileArg, entryPointName, programArgs);

        return await rootCommand.InvokeAsync(args);
    }

    private static void Entry(FileInfo targetFile, string entryPointName, string[] programArgs)
    {
        ProgramRuntimeHandler programRuntimeHandler = new ProgramRuntimeHandler();
        Result<object> result = programRuntimeHandler.RunFile(targetFile, entryPointName, programArgs, System.Console.OpenStandardInput(), System.Console.OpenStandardOutput());
        result.Success(r => System.Console.WriteLine($"Program finished with result: {r}"));
        result.Failure(e => System.Console.WriteLine($"Program encountered an uncaught error during execution: {e}"));
    }

    private static void ValidateCrumpetFile(ArgumentResult arg)
    {
        FileInfo? fileInfo = arg.GetValueOrDefault<FileInfo>();
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (fileInfo is null || !fileInfo.Exists)
        {
            arg.ErrorMessage = "File does not exist";
            return;
        }

        if (fileInfo.Extension != ".crm")
        {
            arg.ErrorMessage = "Invalid file extension";
            return;
        }

        arg.ErrorMessage = null;
    }
}