using System.Net.NetworkInformation;
using System.Net.Sockets;
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

    [CommandOption("map", 'm', Description = "Only shows the link to see the approximate location on a map.", IsRequired = false)]
    public bool Map { get; init; } = false;

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

            if (Map)
            {
                Console.WriteLine("Map link:");
                Console.ForegroundColor = ConsoleColor.Cyan;
                var lat = ip.Lat; var lon = ip.Lon;
                Console.WriteLine($"https://www.openstreetmap.org/directions?engine=graphhopper_foot&route={lat}%2C{lon}%3B{lat}%2C{lon}#map=12/{lat}/{lon}\"");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine($"Detailed information\n");
                Console.WriteLine(ip.ToString());
            }
        }
        catch (Exception ex)
        {
            throw new CommandException(ex.Message);
        }
    }
}

[Command("ip config", Description = "Retrieves information about your IP Config.")]
public class IpConfigCommand() : ICommand
{
    public async ValueTask ExecuteAsync(IConsole console)
    {
        try
        {
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            for (int i = 0; i < networkInterfaces.Length; i++)
            {
                var props = networkInterfaces[i].GetIPProperties();
                WindowsIpConfig config = new
                (
                    networkInterfaces[i].Name,
                    props.UnicastAddresses.FirstOrDefault(x => x.Address.AddressFamily == AddressFamily.InterNetwork)?.Address.ToString(),
                    props.UnicastAddresses.FirstOrDefault(x => x.Address.AddressFamily == AddressFamily.InterNetwork)?.IPv4Mask.ToString(),
                    props.GatewayAddresses.FirstOrDefault(x => x.Address.AddressFamily == AddressFamily.InterNetwork)?.Address.ToString(),
                    props.UnicastAddresses.FirstOrDefault(x => x.Address.AddressFamily == AddressFamily.InterNetworkV6)?.Address.ToString(),
                    props.GatewayAddresses.FirstOrDefault(x => x.Address.AddressFamily == AddressFamily.InterNetworkV6)?.Address.ToString(),
                    props.DnsSuffix,
                    networkInterfaces[i].OperationalStatus
                );
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"{config.Name}\n{string.Concat(Enumerable.Repeat("=", config.Name.Length))}\n");
                Console.ResetColor();
                Console.WriteLine(config.ToString());
                Console.WriteLine("");
            }
        }
        catch (Exception ex)
        {
            throw new CommandException(ex.Message);
        }
    }
}