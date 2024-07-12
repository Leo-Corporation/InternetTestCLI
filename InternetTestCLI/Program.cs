using CliFx;

public static class Program
{
    public static async Task<int> Main() =>
        await new CliApplicationBuilder()
            .SetTitle("InternetTest CLI")
            .SetDescription("InternetTest CLI is a command line interface that provides the same features of InternetTest Pro. It can locate IP addresses, send ping request, get DNS information and more!")
            .AddCommandsFromThisAssembly()
            .Build()
            .RunAsync();
}