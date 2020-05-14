using System;
using System.Reflection;
using ManagementApi;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SmartFace.Cli.Commands;
using SmartFace.Cli.Common;
using SmartFace.Cli.Common.DI;
using SmartFace.Cli.Common.Utils;
using SmartFace.ODataClient.Extensions;

namespace SmartFace.Cli
{
    [Command(Name = "sfcli", Description = "CLI for SmartFace instance"),
     Subcommand(typeof(QueryCmd)),
     Subcommand(typeof(CameraCmd)),
     Subcommand(typeof(WatchlistMemberCmd)),
     Subcommand(typeof(NotificationsCmd)),
    ]
    public class MainCmd : BasicArgumentSolverCmd
    {
        public MainCmd(ILogger<MainCmd> log)
        {
            Log = log;
        }

        private ILogger<MainCmd> Log { get; }

        public static int Main(string[] args)
        {
            try
            {
                return Execute(args);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                return Constants.EXIT_CODE_GENERAL_ERROR;
            }
        }

        private static int Execute(string[] args)
        {
            using var services = Installer.Configure(args);
            var app = services.GetService<CommandLineApplication<MainCmd>>();

            try
            {
                app.Configure(services);
                return app.Execute(args);
            }
            catch (Exception e)
            {
                HandleException(e, app);
                return Constants.EXIT_CODE_GENERAL_ERROR;
            }
        }

        private static void HandleException(Exception e, CommandLineApplication<MainCmd> commandLineApplication)
        {
            switch (e)
            {
                case CommandParsingException _:
                case ApiException _:
                case ProcessingException _:
                    commandLineApplication.Model.Log.LogError(e.Message);
                    break;
                case TargetInvocationException targetInvocationException:
                    HandleException(targetInvocationException.InnerException, commandLineApplication);
                    break;
                case ServerException serverException:
                    commandLineApplication.Model.Log.LogError(serverException.ToString());
                    break;
                default:
                    commandLineApplication.Model.Log.LogError(e, "Unexpected exception");
                    break;
            }
        }

        protected override int OnExecute(CommandLineApplication app, IConsole console)
        {
            return Constants.EXIT_CODE_OK;
        }
    }
}