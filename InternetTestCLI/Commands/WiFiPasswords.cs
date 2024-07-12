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
            LoadWiFiInfo(FileSys.AppDataPath + @"\Léo Corporation\InternetTest CLI\WiFis", Name);

        }
        catch (Exception ex)
        {
            throw new CommandException(ex.Message);
        }
    }

    internal async Task GetWiFiNetworksInfo()
    {

        // Check if the temp directory exists
        string path = FileSys.AppDataPath + @"\Léo Corporation\InternetTest CLI\WiFis";

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