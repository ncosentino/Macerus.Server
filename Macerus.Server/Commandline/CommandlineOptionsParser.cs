using System;
using System.IO;
using Plossum.CommandLine;

namespace Macerus.Server.Commandline
{
    public sealed class CommandlineOptionsParser : ICommandlineOptionsParser
    {
        #region Fields
        private readonly TextWriter _outputWriter;
        #endregion

        #region Constructors
        private CommandlineOptionsParser(TextWriter outputWriter)
        {
            _outputWriter = outputWriter;
        }
        #endregion

        #region Methods
        public static ICommandlineOptionsParser Create(TextWriter outputWriter)
        {
            var parser = new CommandlineOptionsParser(outputWriter);
            return parser;
        }

        public Tuple<int, IStartupParameters> Parse(string[] args)
        {
            var options = StartupParameters.Create();

            var parser = new CommandLineParser(options);
            parser.Parse();

            const int MAX_LINE_LENGTH = 78;
            _outputWriter.WriteLine(parser.UsageInfo.GetHeaderAsString(MAX_LINE_LENGTH));

            if (options.Help)
            {
                _outputWriter.WriteLine(parser.UsageInfo.GetOptionsAsString(MAX_LINE_LENGTH));
                return new Tuple<int, IStartupParameters>(1, options);
            }

            if (parser.HasErrors)
            {
                _outputWriter.WriteLine(parser.UsageInfo.GetErrorsAsString(MAX_LINE_LENGTH));
                return new Tuple<int, IStartupParameters>(-1, options);
            }

            return new Tuple<int, IStartupParameters>(0, options);
        }
        #endregion
    }
}
