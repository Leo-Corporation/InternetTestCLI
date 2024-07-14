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
using CliFx;
using CliFx.Attributes;
using CliFx.Exceptions;
using CliFx.Infrastructure;
using InternetTestCLI.Classes;

namespace InternetTestCLI.Commands;

[Command("trace", Description = "Executes a traceroute for a provided website.")]
public class TracerouteCommand() : ICommand
{
    [CommandParameter(0, Name = "site", Description = "Site URL.")]
    public required string Site { get; init; }

    [CommandParameter(1, Name = "max_hops", Description = "The maximum amount of hops.", IsRequired = false)]
    public int MaxHops { get; init; } = 30;



    public async ValueTask ExecuteAsync(IConsole Console)
    {
        try
        {
            Console.Output.WriteLine($"Traceroute in progress for {Site}, please wait...");

            int success = 0; int failed = 0; long time = 0;
            var route = await Trace(Site, MaxHops, 5000);

            for (int i = 0; i < route.Count; i++)
            {

                if (route[i].Status == IPStatus.Success || route[i].Status == IPStatus.TtlExpired) success++;
                else failed++;
                time += route[i].RoundtripTime;
            }

            Console.Output.WriteLine("Overview");
            Console.Output.WriteLine("========\n");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Output.WriteLine($"Success: {success}");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Output.WriteLine($"Failed: {failed}");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Output.WriteLine($"Time: {time}ms");
            Console.ResetColor();

        }
        catch (Exception ex)
        {
            throw new CommandException(ex.Message);
        }
    }

    public static async Task<List<TracertStep>> Trace(string target, int maxHops, int timeout)
    {
        List<TracertStep> steps = [];

        for (int ttl = 1; ttl <= maxHops; ttl++)
        {
            PingReply reply = await TraceRoute(target, ttl, timeout);

            TracertStep step = new()
            {
                TTL = ttl,
                Address = reply.Address,
                RoundtripTime = reply.RoundtripTime,
                Status = reply.Status
            };

            steps.Add(step);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"#{steps.Count} - {step.Address}\n{string.Join("", Enumerable.Repeat("=", $"#{steps.Count} - {step.Address}".Length))}");
            Console.ResetColor();
            Console.WriteLine($"\n\tStatus: {step.Status}\n\tRoundrip time: {step.RoundtripTime}\n\tTTL: {step.TTL}\n");

            if (reply.Status == IPStatus.Success)
                break;
        }

        return steps;
    }

    public static Task<PingReply> TraceRoute(string targetAddress, int ttl, int timeout)
    {
        using Ping pingSender = new();
        PingOptions options = new()
        {
            Ttl = ttl
        };

        byte[] buffer = new byte[32];
        return pingSender.SendPingAsync(targetAddress, timeout, buffer, options);
    }
}