using CliFx;
using CliFx.Attributes;
using CliFx.Exceptions;
using CliFx.Infrastructure;
using InternetTestCLI.Classes;

namespace InternetTestCLI.Commands;

[Command("ip", Description = "Retrieves information about your public IP address.")]
public class MyIpCommand() : ICommand
{
    public async ValueTask ExecuteAsync(IConsole console)
    {
        try
        {
            Console.WriteLine($"Getting information for your public IP, please wait...");
            var ip = await IPInfo.GetIPInfoAsync("");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Your public IP address is {ip.Query}");
            Console.ResetColor();
            Console.WriteLine("");

            Console.WriteLine($"Detailed information\n");
            Console.WriteLine(ip.ToString());
        }
        catch (Exception ex)
        {
            throw new CommandException(ex.Message);
        }
    }
}

[Command("ip locate", Description = "Retrieves information about an IP address with its position.")]
public class LocateIpCommand() : ICommand
{
    [CommandParameter(0, Name = "ip", Description = "The IP address to locate.")]
    public required string IP { get; init; }
    public async ValueTask ExecuteAsync(IConsole console)
    {
        try
        {
            Console.WriteLine($"Getting information for the requested IP, please wait...");
            var ip = await IPInfo.GetIPInfoAsync(IP);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"IP address is {ip.Query}");
            Console.ResetColor();
            Console.WriteLine("");

            Console.WriteLine($"Detailed information\n");
            Console.WriteLine(ip.ToString());
        }
        catch (Exception ex)
        {
            throw new CommandException(ex.Message);
        }
    }
}