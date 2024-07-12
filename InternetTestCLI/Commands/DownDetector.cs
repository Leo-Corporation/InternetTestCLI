using CliFx;
using CliFx.Attributes;
using CliFx.Exceptions;
using CliFx.Infrastructure;
using PeyrSharp.Core;

namespace InternetTestCLI.Commands;

[Command("downdetector", Description = "Checks if a website is down.")]
public class DownDetectorTestCommand() : ICommand
{
    [CommandParameter(0, Name = "site", Description = "Site URL.")]
    public required string Site { get; init; }

    public async ValueTask ExecuteAsync(IConsole Console)
    {
        try
        {
            Console.Output.WriteLine($"Test in progress for {Site}, please wait...");


            var statusInfo = await Internet.GetStatusInfoAsync(Site); // Makes a request to the specified website and saves the status code and message
            var color = statusInfo.StatusCode switch
            {
                var code when code >= 100 && code < 200 => ConsoleColor.Blue,// Informational
                var code when code >= 200 && code < 300 => ConsoleColor.Green,// Success
                var code when code >= 300 && code < 400 => ConsoleColor.Yellow,// Redirection
                var code when code >= 400 && code < 500 => ConsoleColor.Red,// ClientError
                var code when code >= 500 && code < 600 => ConsoleColor.DarkRed,// ServerError
                _ => Console.ForegroundColor,// Keep the default color for unexpected values
            };
            // Load Information
            Console.Output.Write("Status Code: "); Console.ForegroundColor = color; Console.Output.Write(statusInfo.StatusCode); Console.ResetColor();
            Console.Output.WriteLine("");
            Console.Output.WriteLine($"Status Type: {statusInfo.StatusType}");
            Console.Output.Write($"Status Message: "); Console.ForegroundColor = color; Console.Output.Write(statusInfo.StatusDescription + "\n"); Console.ResetColor();
        }
        catch (Exception ex)
        {
            throw new CommandException(ex.Message);
        }
    }
}