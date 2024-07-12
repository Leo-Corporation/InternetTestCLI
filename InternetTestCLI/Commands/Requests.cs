using CliFx;
using CliFx.Attributes;
using CliFx.Exceptions;
using CliFx.Infrastructure;
using RestSharp;

namespace InternetTestCLI.Commands;

[Command("request", Description = "Makes a request to the specified resource.")]
public class RequestCommand() : ICommand
{
    [CommandParameter(0, Name = "method", Description = "The method to use when executing the request.")]
    public required Method Method { get; init; } = Method.Get;

    [CommandParameter(1, Name = "url", Description = "The URL where to send the request.")]
    public required string URL { get; init; }

    [CommandOption("content", 'c', Description = "Only outputs the content of the response.", IsRequired = false)]
    public bool ContentOnly { get; init; } = false;

    public async ValueTask ExecuteAsync(IConsole Console)
    {
        try
        {
            Console.Output.WriteLine($"Executing a {Method} request for {URL}, please wait...");
            await ExecuteRequest(Method, URL);


        }
        catch (Exception ex)
        {
            throw new CommandException(ex.Message);
        }
    }

    public async Task ExecuteRequest(Method method, string url)
    {
        var options = new RestClientOptions(url);
        var client = new RestClient(options);
        var request = new RestRequest("", method);

        var response = await client.ExecuteAsync(request);
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\nResponse Content");
        Console.WriteLine("================\n");
        Console.ResetColor();
        Console.WriteLine(response.Content);

        if (ContentOnly) return;

        string headers = "";
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\nHeaders");
        Console.WriteLine("=======\n");

        foreach (var item in response.Headers)
        {
            headers += item.ToString() + "\n";
            var header = item.ToString().Split("=", 2);
            if (header.Length < 2) continue;

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(header[0]);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(header[1] + "\n");
            Console.ResetColor();
        }
    }
}