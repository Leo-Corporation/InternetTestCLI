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
using InternetTestCLI.Classes;
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