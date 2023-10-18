// See https://aka.ms/new-console-template for more information
using CommandLine;
using ConfigComparer;
using Microsoft.Extensions.Configuration;

Console.WriteLine("Config Comparer");

Parser.Default.ParseArguments<CompareOptions>(args)
    .WithParsed<CompareOptions>(o =>
    {
        IConfigurationRoot BuildSource(string directory)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            foreach (var fileName in o.Files)
            {
                string path = Path.Combine(directory, fileName);
                bool exists = File.Exists(path);
                Console.WriteLine($"- JSON file: {path} [{(exists ? "found" : "not found")}]");
                if (exists) {
                    builder = builder.AddJsonFile(path, optional: false);
                }
            }
            return builder.Build();
        }

        var projector = new ConfigurationProjector();

        Console.WriteLine("Adding left configuration files.");
        var left = projector.Project(BuildSource(o.LeftDirectory));
        Console.WriteLine("Adding right configuration files.");
        var right = projector.Project(BuildSource(o.RightDirectory));

        Console.WriteLine("---- diff start");

        var zipComparer = new ZipComparer(
            keyComparer: StringComparer.OrdinalIgnoreCase,
            valueComparer: StringComparer.Ordinal
        );

        var consoleWriter = new ConsoleCompareWriter()
        {
            HideSame = o.SkipSame,
            HideValues = o.NoValues
        };

        var lines = zipComparer.Compare(left, right);
        consoleWriter.Write(lines);
        Console.WriteLine("---- diff end");
        consoleWriter.WriteStatsToConsole();
        Console.WriteLine("Done.");
    });