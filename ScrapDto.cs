using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapFile
{
    public class Options
    {
        public virtual string InputFile { get; set; }
        public virtual string OutputFile { get; set; }
        public virtual string Separator { get; set; }
        public virtual string ReplaceSeparator { get; set; }
        public virtual string SearchStrValue { get; set; }
    }

    [Verb("ScrapFile", isDefault: true, HelpText = "Extract from any file to csv")]
    public class ScrapDto : Options
    {
        [Option('i', "input", Required = true, HelpText = "Input file path")]
        public override string InputFile { get; set; }

        [Option('o', "output", Required = true, HelpText = "Output file path")]
        public override string OutputFile { get; set; }

        [Option('s', "separator", Required = true, HelpText = "Separator")]
        public override string Separator { get; set; }

        [Option('r', "replaceSeparator", Required = true, HelpText = "Replace Separator")]
        public override string ReplaceSeparator { get; set; }

        [Option('v', "searchStrValue", Required = true, HelpText = "Search String Value")]
        public override string SearchStrValue { get; set; }

    }
}
