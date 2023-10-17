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
    }
}
