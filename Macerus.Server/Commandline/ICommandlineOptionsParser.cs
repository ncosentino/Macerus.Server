using System;

namespace Macerus.Server.Commandline
{
    public interface ICommandlineOptionsParser
    {
        Tuple<int, IStartupParameters> Parse(string[] args);
    }
}