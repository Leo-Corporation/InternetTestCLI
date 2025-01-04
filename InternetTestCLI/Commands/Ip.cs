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
using InternetTestCLI.Enums;

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

	[CommandOption("provider", 'p', Description = "The map provider to use.", IsRequired = false)]
	public MapProvider Provider { get; init; } = MapProvider.OpenStreetMap;

	[CommandOption("zoom", 'z', Description = "The zoom level of the map.", IsRequired = false)]
	public int Zoom { get; init; } = 12;

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

				Console.Output.WriteLine(GetMapLink(Provider, lat, lon, Zoom));
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

	/// <summary>
	/// Get the map link for a specific map provider.
	/// </summary>
	/// <param name="lat">The latitude of the point.</param>
	/// <param name="lon">The longitude of the point.</param>
	/// <param name="provider">The map provider to use.</param>
	/// <returns>The link to the map view.</returns>
	public static string GetMapLink(MapProvider provider, string lat, string lon, int zoom)
	{
		return provider switch
		{
			MapProvider.Google => $"\"https://www.google.com/maps/place/{GetGoogleMapsPoint(double.Parse(lat), double.Parse(lon))}\"",
			MapProvider.Microsoft => $"\"https://www.bing.com/maps?q={lat} {lon}&lvl={zoom}&cp={lat}~{lon}\"",
			MapProvider.Here => $"\"https://wego.here.com/directions/mix/{lat},{lon}/?map={lat},{lon},{zoom}\"",
			MapProvider.Yandex => $"\"https://yandex.com/maps/?ll={lon}%2C{lat}&z={zoom}\"",
			_ => $"\"https://www.openstreetmap.org/directions?engine=graphhopper_foot&route={lat}%2C{lon}%3B{lat}%2C{lon}#map={zoom}/{lat}/{lon}\"",
		};
	}

	/// <summary>
	/// Get the point for the Google Maps map provider.
	/// </summary>
	/// <param name="lat">The latitude of the point.</param>
	/// <param name="lon">The longitude of the point.</param>
	/// <returns>A <see cref="string"/> value like <c>XX° XX.XXX' N/S, XX° XX' E/W</c>.</returns>
	public static string GetGoogleMapsPoint(double lat, double lon)
	{
		int deg = (int)lat; // Get integer
		int deg2 = (int)lon; // Get integer

		double d = (lat - deg) * 60d;
		double d2 = (lon - deg2) * 60d;

		string fDir = lat >= 0 ? "N" : "S"; // Get if the location is in the North or South
		string sDir = lon >= 0 ? "E" : "W"; // Get if the location is in the East or West

		string sD = d.ToString().Replace(",", "."); // Ensure to use . instead of ,
		string sD2 = d2.ToString().Replace(",", "."); // Ensure to use . instead of ,

		return $"{deg}° {sD}' {fDir}, {deg2}° {sD2}' {sDir}".Replace("-", "");
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