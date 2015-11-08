using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Macerus.Server
{
    public interface IStartupParameters
    {
        #region Properties
        bool Help { get; }

        string HostName { get; }

        int Port { get; }
        #endregion
    }
}
