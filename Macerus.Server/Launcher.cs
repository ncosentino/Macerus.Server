using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ProjectXyz.Api.Amqp;
using ProjectXyz.Api.Core;
using ProjectXyz.Api.Interface;
using ProjectXyz.Data.Sql;
using ProjectXyz.Game.Core;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Macerus.Server
{
    public sealed class Launcher : ILauncher
    {
        #region Constructors
        private Launcher()
        {
        }
        #endregion

        #region Methods
        public static ILauncher Create()
        {
            var launcher = new Launcher();
            return launcher;
        }

        public async Task Launch(
            IStartupParameters startupParameters, 
            CancellationToken cancellationToken)
        {
            var factory = new ConnectionFactory()
            {
                HostName = startupParameters.HostName,
                Port = startupParameters.Port,
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(
                    queue: "hello",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var notifier = (INotifier)null; // TODO: implement this
                var responder = (IResponder)null; // TODO: implement this

                var consumer = new EventingBasicConsumer(channel);
                var requestFactory = RequestFactory.Create();

                using (var requestPublisher = RequestPublisher.Create(
                    consumer,
                    requestFactory,
                    Environment.ProcessorCount,
                    100))
                using (var requestRegistrar = RequestRegistrar.Create(requestPublisher))
                using (var databaseConnection = new SQLiteConnection(CreateSqlConnectionString()))
                {
                    databaseConnection.Open();

                    var database = SqlDatabase.Create(
                        databaseConnection,
                        false);
                    var databaseUpgrader = SqlDatabaseUpgrader.Create();
                    var dataManager = SqlDataManager.Create(
                        database,
                        databaseUpgrader);
                    var apiManager = ApiManager.Create(
                        requestRegistrar,
                        notifier,
                        responder);
                    var gameManager = GameManager.Create(
                        database,
                        dataManager,
                        GetPluginDirectories());
                    var game = Game.Create(
                        gameManager,
                        apiManager);

                    await game.StartAsync(cancellationToken);
                }
            }
        }

        private IEnumerable<string> GetPluginDirectories()
        {
            yield return AppDomain.CurrentDomain.BaseDirectory;
        }

        private string CreateSqlConnectionString()
        {
            return "Data Source=\":memory:\"";
        }
        #endregion
    }
}
