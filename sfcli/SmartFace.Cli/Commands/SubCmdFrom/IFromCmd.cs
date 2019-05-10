using System.Collections;
using McMaster.Extensions.CommandLineUtils;

namespace SmartFace.Cli.Commands.SubCmdFrom
{
    public interface IFromCmd
    {
        IEnumerable Execute(IConsole console);
    }
}