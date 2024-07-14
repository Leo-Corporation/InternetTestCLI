/*
MIT License

Copyright (c) Léo Corporation

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE. 
*/

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
    public async ValueTask ExecuteAsync(IConsole Console)
    {
        try
        {
            Console.Output.WriteLine($"Getting information for your public IP, please wait...");
            var ip = await IPInfo.GetIPInfoAsync("");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Output.WriteLine($"Your public IP address is {ip.Query}");
            Console.ResetColor();
            Console.Output.WriteLine("");

            Console.Output.WriteLine($"Detailed information\n");
            Console.Output.WriteLine(ip.ToString());
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

    public async ValueTask ExecuteAsync(IConsole Console)
    {

        try
        {
            Console.Output.WriteLine($"Getting information for the requested IP, please wait...");
            var ip = await IPInfo.GetIPInfoAsync(IP);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Output.WriteLine($"IP address is {ip.Query}");
            Console.ResetColor();
            Console.Output.WriteLine("");

            if (Map)
            {
                Console.Output.WriteLine("Map link:");
                Console.ForegroundColor = ConsoleColor.Cyan;
                var lat = ip.Lat.ToString().Replace(",", "."); var lon = ip.Lon.ToString().Replace(",", ".");

                Console.Output.WriteLine($"https://www.openstreetmap.org/directions?engine=graphhopper_foot&route={lat}%2C{lon}%3B{lat}%2C{lon}#map=12/{lat}/{lon}\"");
                Console.ResetColor();
            }
            else
            {
                Console.Output.WriteLine($"Detailed information\n");
                Console.Output.WriteLine(ip.ToString());
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
    public async ValueTask ExecuteAsync(IConsole Console)
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
                Console.Output.WriteLine($"{config.Name}\n{string.Concat(Enumerable.Repeat("=", config.Name.Length))}\n");
                Console.ResetColor();
                Console.Output.WriteLine(config.ToString());
                Console.Output.WriteLine("");
            }
        }
        catch (Exception ex)
        {
            throw new CommandException(ex.Message);
        }
    }
}