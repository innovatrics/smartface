using System;
using McMaster.Extensions.CommandLineUtils;

namespace SmartFace.Cli.Common.Utils
{
    public static class CommandLineApplicationExtensions
    {
        public static void Configure(this CommandLineApplication app, IServiceProvider serviceProvider)
        {
            app.Conventions.UseConstructorInjection(serviceProvider);
            app.Conventions.UseDefaultConventions();
        }
        public static void ConfigureAsArgumentSolver(this CommandLineApplication app)
        {
            app.Conventions.UseConstructorInjection();
            app.Conventions.UseDefaultConventions();
            app.UnrecognizedArgumentHandling = UnrecognizedArgumentHandling.StopParsingAndCollect;
            app.DisableHelpOption();
        }

        private static void DisableHelpOption(this CommandLineApplication app)
        {
            app.OptionHelp.LongName = new Guid("65B7B0CA-E186-4D8B-AF0A-D629BC6BA614").ToString();
            app.OptionHelp.ShortName = new Guid("54D69F24-7A6B-42AF-844F-186BE16F671F").ToString();
        }
    }
}
