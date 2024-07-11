using System.Net.NetworkInformation;
using CliFx;
using CliFx.Attributes;
using CliFx.Exceptions;
using CliFx.Infrastructure;
using PeyrSharp.Core;

namespace InternetTestCLI.Commands;

[Command("ping", Description = "Makes a ping request to a URL.")]
public class PingCommand() : ICommand
{
    [CommandParameter(0, Name = "site", Description = "Site to ping.")]
    public required string Site { get; init; }

    [CommandOption("amount", 'a', Description = "The number of ping requests to make.", IsRequired = false)]
    public int Amount { get; init; } = 4;

    public async ValueTask ExecuteAsync(IConsole Console)
    {
        try
        {
            Console.Output.WriteLine($"Pinging {Site}...\n");
            int sent = 0, received = 0;

            long[] times = new long[Amount]; // Create an array
            for (int i = 0; i < Amount; i++)
            {
                var ping = await new Ping().SendPingAsync(Site); // Send a ping
                sent++;
                times[i] = ping.RoundtripTime; // Get the time of the ping

                string nl = $"{i + 1}/{Amount}"; // Add a new line if it's not the last ping
                Console.Output.WriteLine($"{nl}. Pinging: {ping.Address} ({ping.RoundtripTime}ms)");

                if (ping.Status == IPStatus.Success)
                {
                    received++;
                }
            }

            Console.Output.WriteLine("");

            // Get the average, minimum, and maximum of the times and print them
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Output.WriteLine($"Average Time: {times.Average():0.00}ms");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Output.WriteLine($"Min Time: {times.Min()}ms");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Output.WriteLine($"Max Time: {times.Max()}ms");
            Console.ResetColor();

            Console.Output.WriteLine("");

            // Print the number of sent, received, and lost pings
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Output.WriteLine($"Sent: {sent}");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Output.WriteLine($"Received: {received}");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Output.WriteLine($"Lost: {sent - received}");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            throw new CommandException(ex.Message);
        }
    }
}