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

#if _WINDOWS
using System.Diagnostics;
using System.Xml.Serialization;
using CliFx;
using CliFx.Attributes;
using CliFx.Exceptions;
using CliFx.Infrastructure;
using InternetTestCLI.Classes;
using PeyrSharp.Env;



namespace InternetTestCLI.Commands;

[Command("wifi password", Description = "Retrieves your saved WiFi Passwords.")]
public class WiFiPasswordCommand() : ICommand
{
    [CommandOption("name", 'n', Description = "The name of specific network to retrieve.", IsRequired = false)]
    public string Name { get; init; } = "";
    public async ValueTask ExecuteAsync(IConsole Console)
    {
        try
        {
            Console.Output.WriteLine($"Retrieving your WiFi Passwords, please wait...\n");

            await GetWiFiNetworksInfo();
            LoadWiFiInfo(FileSys.AppDataPath + @"\LÃ©o Corporation\InternetTest CLI\WiFis", Name);

        }
        catch (Exception ex)
        {
            throw new CommandException(ex.Message);
        }
    }

    internal async Task GetWiFiNetworksInfo()
    {

        // Check if the temp directory exists
        string path = FileSys.AppDataPath + @"\LÃ©o Corporation\InternetTest CLI\WiFis";

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        };

        // Run "netsh wlan export profile key=clear" command
        Process process = new();
        process.StartInfo.FileName = "cmd.exe";
        process.StartInfo.Arguments = $"/c netsh wlan export profile key=clear folder=\"{path}\"";
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        process.Start();
        await process.WaitForExitAsync();
    }

    internal void LoadWiFiInfo(string path, string name = "")
    {
        string[] files = Directory.GetFiles(path);
        for (int i = 0; i < files.Length; i++)
        {
            XmlSerializer serializer = new(typeof(WLANProfile));
            StreamReader streamReader = new(files[i]); // Where the file is going to be read

            var test = (WLANProfile?)serializer.Deserialize(streamReader);

            if (test != null && test.Name.Contains(name))
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(test.Name);
                Console.WriteLine(string.Join("", Enumerable.Repeat("=", test.Name.Length)));
                Console.ResetColor();

                Console.WriteLine(test.ToString() + '\n');
            }
            streamReader.Close();
        }
    }

}
#endif