using System.Threading;
using System.Threading.Tasks;

namespace Macerus.Server
{
    public interface ILauncher
    {
        Task Launch(
            IStartupParameters startupParameters, 
            CancellationToken cancellationToken);
    }
}