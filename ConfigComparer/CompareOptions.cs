using CommandLine.Text;
using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigComparer
{
    public class CompareOptions
    {
        [Value(0, Required = true, HelpText = "Path to the left compare directory with JSON config files.")]
        public string LeftDirectory { get; set; }

        [Value(1, Required = true, HelpText = "Path to the right compare directory with JSON config files.")]
        public string RightDirectory { get; set; }

        [Value(2, HelpText = "List of config files to read (for example: appsettings.json appsettings.Production.json)")]
        public IEnumerable<string> Files { get; set; }

        [Option('s', "skip-same", HelpText = "If set, only non-equal configuration is printed.")]
        public bool SkipSame { get; set; }

        [Option('n', "no-values", HelpText = "If set, values are not shown.")]
        public bool NoValues { get; set; }

        [Usage(ApplicationAlias = "config-comparer")]
        public static IEnumerable<Example> Examples
        {
            get
            {
                return new List<Example>() {
                    new Example(
                        "Compare two folders", 
                        new CompareOptions { 
                            LeftDirectory = "c:\\project-a", 
                            RightDirectory = "c:\\project-b", 
                            Files = new string[] { "appsettings.json", "appsettings.Production.json" } 
                        }),
                    new Example(
                        "Compare two folders. Show only differences",
                        new CompareOptions {
                            LeftDirectory = "c:\\project-a",
                            RightDirectory = "c:\\project-b",
                            SkipSame = true,
                            Files = new string[] { "appsettings.json", "appsettings.Production.json" }
                        }),
                    new Example(
                        "Compare two folders. Show only differences and hide real values",
                        new CompareOptions {
                            LeftDirectory = "c:\\project-a",
                            RightDirectory = "c:\\project-b",
                            SkipSame = true,
                            NoValues = true,
                            Files = new string[] { "appsettings.json", "appsettings.Production.json" }
                        })
                };
            }
        }
    }
}
