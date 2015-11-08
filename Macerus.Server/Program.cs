using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Macerus.Server.Commandline;

namespace Macerus.Server
{
    internal sealed class Program
    {
        #region Methods
        private static int Main(string[] args)
        {
            var commandlineOptionsParser = CommandlineOptionsParser.Create(Console.Out);
            var optionsResult = commandlineOptionsParser.Parse(args);
            if (optionsResult.Item1 != 0)
            {
                return optionsResult.Item1;
            }

            var launcher = Launcher.Create();
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                var gameTask = launcher.Launch(
                    optionsResult.Item2,
                    cancellationTokenSource.Token);

                Console.CancelKeyPress += (_, e) =>
                {
                    Console.WriteLine("Cancellation requested. Terminating...");
                    cancellationTokenSource.Cancel();
                    e.Cancel = true;
                };
                Console.WriteLine("Press CTRL+C to exit.");

                try
                {
                    gameTask.Wait(cancellationTokenSource.Token);
                }
                catch (OperationCanceledException)
                {
                }
            }

            Console.WriteLine("Server terminated.");
            return 0;
        }
        #endregion
    }
}
