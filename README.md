![GitHub contributors](https://img.shields.io/github/contributors/Leo-Corporation/InternetTestCLI)
![GitHub issues](https://img.shields.io/github/issues/Leo-Corporation/InternetTestCLI)
![GitHub](https://img.shields.io/github/license/Leo-Corporation/InternetTestCLI)
![GitHub Workflow Status](https://img.shields.io/github/actions/workflow/status/Leo-Corporation/InternetTestCLI/dotnet-core-desktop.yml?branch=main)
![Using PeyrSharp](https://img.shields.io/badge/using-PeyrSharp-DD00FF?logo=nuget)
![GitHub release (latest by date)](https://img.shields.io/github/v/release/Leo-Corporation/InternetTestCLI)
<br />

<p align="center">
  <a href="https://github.com/Leo-Corporation/InternetTestCLI">
    <img src=".github/images/logo.png" alt="Logo" width="80" height="80">
  </a>

  <h1 align="center">InternetTest CLI</h3>

  <p align="center">
    Network-related command line tools.<br /> InternetTest CLI is a command line interface that provides the same features of InternetTest Pro. It can locate IP addresses, send ping request, get DNS information and more!
    <br />
    <a href="https://github.com/Leo-Corporation/InternetTestCLI/issues/new?assignees=&labels=bug&template=bug-report.yml&title=%5BBug%5D+">Report Bug</a>
    ·
    <a href="https://github.com/Leo-Corporation/InternetTestCLI/issues/new?assignees=&labels=enhancement&template=feature-request.yml&title=%5BEnhancement%5D+">Request Feature</a>
    ·
    <a href="https://github.com/Leo-Corporation/InternetTestCLI/releases">Releases</a>

  </p>
</p>

## Introduction

![Banner](.github/images/banner.png)

InternetTest CLI is a command line interface that provides the same features as InternetTest Pro. It can locate IP addresses, send ping requests, get DNS information, and more!

## Features

- Locate IP addresses
- Send ping requests
- Get DNS information
- Check if a website is down
- Retrieve public IP address information
- Execute traceroutes
- Retrieve saved WiFi passwords

## Installation

[Click here](https://github.com/Leo-Corporation/InternetTestCLI/releases) to download the latest release of InternetTest CLI.

## Usage

```sh
itcli [options]
itcli [command] [...]
```

### Options

- `-h` or `--help` Shows help text.
- `--version` Shows version information.

### Commands

- `dns` Gets DNS information about a domain name.
- `downdetector` Checks if a website is down.
- `ip` Retrieves information about your public IP address. Subcommands: `ip config`, `ip locate`.
- `ping` Makes a ping request to a URL.
- `request` Makes a request to the specified resource.
- `trace` Executes a traceroute for a provided website.
- `update` Checks if a newer version of InternetTest CLI is available.
- `wifi password` Retrieves your saved WiFi passwords.

## Examples

Get DNS information for a domain:

```sh
itcli dns example.com
```

Check if a website is down:

```sh
itcli downdetector example.com
```

Retrieve public IP address information:

```sh
itcli ip locate [ip address]
```

Send a ping request to a URL:

```sh
itcli ping example.com
```

Execute a traceroute for a website:

```sh
itcli trace example.com
```

Retrieve your saved WiFi passwords (Windows only):

```sh
itcli wifi password
```

## Help

You can run `itcli [command] --help` to show help on a specific command.

```sh
itcli dns --help
```

## Contributing

Thank you for considering contributing to InternetTest CLI! We welcome contributions from the community to help us improve and enhance this utility.

Before you begin, please take a moment to review our [Contribution Guidelines](CONTRIBUTING.md) to understand our process and expectations. These guidelines cover various aspects of contributing, including reporting issues, suggesting new features, and submitting code changes.

### Requirements

Before you begin contributing to InternetTest CLI, ensure that you have the following prerequisites installed:

- Visual Studio Code or Visual Studio 2022
- .NET 8 SDK and Runtime
- Git

These tools and software components are essential for building, testing, and contributing to InternetTest CLI effectively. Make sure to have them set up on your development environment before proceeding with any contributions.

### How to Contribute

1. **Check for Issues:** Before starting work on a new feature or bug fix, check the [issue tracker](https://github.com/Leo-Corporation/InternetTest/issues) to see if it has already been reported or is being worked on.

2. **Fork the Repository:** If you plan to contribute, fork the InternetTest CLI repository to your GitHub account.

3. **Create a Branch:** Create a new branch in your forked repository for your work. Choose a descriptive name that reflects the purpose of your contribution.

4. **Make Changes:** Make your changes or additions in your branch. Be sure to follow the coding style and guidelines outlined in our [Contributor Guidelines](CONTRIBUTING.md).

5. **Testing:** Test your changes thoroughly to ensure they don't introduce any unintended side effects.

6. **Submit a Pull Request:** When you're confident in your changes, submit a pull request to the main InternetTest CLI repository. Be sure to provide a clear and concise explanation of your changes and the problem they address.

### Code of Conduct

Please note that by contributing to InternetTest CLI, you are expected to follow our [Code of Conduct](CODE_OF_CONDUCT.md). This ensures a respectful and inclusive environment for everyone involved in the project.

We appreciate your contributions and look forward to working together to improve InternetTest CLI for the community. Your input helps us make the utility better and more effective for users worldwide.

For more detailed information on how to contribute, refer to our [Contribution Guidelines](CONTRIBUTING.md).

Thank you for being part of the InternetTest CLI community!

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
