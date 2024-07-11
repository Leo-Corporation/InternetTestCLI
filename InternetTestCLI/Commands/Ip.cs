using System.Text.Json;
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
            var ip = await GetIPInfoAsync("");

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

    internal async static Task<IPInfo?> GetIPInfoAsync(string ip)
    {
        HttpClient httpClient = new();
        string result = await httpClient.GetStringAsync($"http://ip-api.com/json/{ip}");

        return JsonSerializer.Deserialize<IPInfo>(result);
    }
}

