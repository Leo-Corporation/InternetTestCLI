using CliFx;
using CliFx.Attributes;
using CliFx.Exceptions;
using CliFx.Infrastructure;
using InternetTestCLI.Classes;
using PeyrSharp.Core;
using PeyrSharp.Env;

namespace InternetTestCLI.Commands;

[Command("update", Description = "Checks if a newer version of InternetTest CLI is available.")]
public class UpdateCommand() : ICommand
{
    public async ValueTask ExecuteAsync(IConsole Console)
    {
        try
        {
            Console.Output.WriteLine($"Checking for updates, please wait...");
            string last = await Update.GetLastVersionAsync("https://raw.githubusercontent.com/Leo-Corporation/LeoCorp-Docs/master/Liens/Update%20System/InternetTest/CLI/Version.txt");
            string download = await Update.GetLastVersionAsync("https://raw.githubusercontent.com/Leo-Corporation/LeoCorp-Docs/master/Liens/Update%20System/InternetTest/CLI/Download.txt");

            if (Update.IsAvailable(Global.Version, last))
            {
                Console.Output.WriteLine("Updates are available for InternetTest CLI.");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Output.WriteLine($"\tVersion: {last}");
                Console.Output.WriteLine($"\tDownload: {download}");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Output.WriteLine("Everything is update-to-date!");
            }
            Console.ResetColor();

        }
        catch (Exception ex)
        {
            throw new CommandException(ex.Message);
        }
    }
}