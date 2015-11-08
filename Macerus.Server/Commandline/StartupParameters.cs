using Plossum.CommandLine;

namespace Macerus.Server.Commandline
{
    [CommandLineManager(
        ApplicationName = "Macerus Server",
        Copyright = "Nick Cosentino",
        Description = "The application server for Macerus.",
        Version = "0.0.0.1")]
    public sealed class StartupParameters : IStartupParameters
    {
        #region Constructors
        private StartupParameters()
        {
            HostName = "localhost";
            Port = 5672;
        }
        #endregion

        #region Properties
        [CommandLineOption(
            Description = "Prints this help text.",
            Name = "Help",
            Aliases = "?")]
        public bool Help { get; set; }

        [CommandLineOption(
            Description = "The host name for this server to bind to.",
            Name = "HostName",
            Aliases = "h host")]
        public string HostName { get; set; }

        [CommandLineOption(
            Description = "The port for this server to bind to.",
            Name = "Port",
            Aliases = "p")]
        public int Port { get; set; }
        #endregion

        #region Methods
        public static IStartupParameters Create()
        {
            var parameters = new StartupParameters();
            return parameters;
        }
        #endregion
    }
}
