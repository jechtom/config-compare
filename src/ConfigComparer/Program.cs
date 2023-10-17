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

        Console.WriteLine("Comparing.");

        var zipComparer = new ZipComparer(
            keyComparer: StringComparer.OrdinalIgnoreCase,
            valueComparer: StringComparer.Ordinal
        );

        int same = 0;
        int diff = 0;
        int leftOnly = 0;
        int rightOnly = 0;

        foreach(var item in zipComparer.Compare(left, right))
        {
            switch (item)
            {
                case CompareLineResult.Same s: 
                    same++;
                    if (!o.SkipSame)
                    {
                        Console.Write($"== [Same] {s.Left.Key} = {s.Left.Value}");
                        if (!o.NoValues)
                        {
                            Console.Write($" = {s.Left.Value}");
                        }
                        Console.WriteLine();
                    }
                    break;
                case CompareLineResult.Different d: 
                    diff++; 
                    Console.Write($"~~ [Different] {d.Left.Key}");
                    if (!o.NoValues)
                    {
                        Console.Write($" = {d.Left.Value} / {d.Right.Value}");
                    }
                    Console.WriteLine();
                    break;
                case CompareLineResult.LeftOnly l: 
                    leftOnly++; 
                    Console.Write($"<< [LeftOnly] {l.Left.Key}");
                    if (!o.NoValues)
                    {
                        Console.Write($" = {l.Left.Value}");
                    }
                    Console.WriteLine();
                    break;
                case CompareLineResult.RightOnly r: 
                    rightOnly++; 
                    Console.Write($">> [RightOnly] {r.Right.Key}");
                    if (!o.NoValues)
                    {
                        Console.Write($" = {r.Right.Value}");
                    }
                    Console.WriteLine();
                    break;
                default:
                    throw new InvalidOperationException("Unknown.");
            }
        }

        Console.WriteLine("End.");
        Console.WriteLine("Statistics:");
        Console.WriteLine($" - Same items: {same}");
        Console.WriteLine($" - Different items: {diff}");
        Console.WriteLine($" - Left/right only: {leftOnly}/{rightOnly}");
        Console.WriteLine("Done.");

    });