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
            string url = Site;
            if (!(url.Contains("https://") || url.Contains("http://")))
            {
                Console.Output.WriteLine("The protocol was not specified, assuming that it is HTTPS.");
                url = "https://" + url;
            }
            Console.Output.WriteLine($"Test in progress for {url}, please wait...");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Output.WriteLine("\nStatus");
            Console.Output.WriteLine("======\n");
            Console.ResetColor();

            var statusInfo = await Internet.GetStatusInfoAsync(url); // Makes a request to the specified website and saves the status code and message
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