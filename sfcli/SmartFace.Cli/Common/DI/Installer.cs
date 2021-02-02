using System;
using System.Net.Http;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SmartFace.Cli.Commands;
using SmartFace.Cli.Common.DI.Factories;
using SmartFace.Cli.Common.Utils;
using SmartFace.Cli.Core.ApiAbstraction;
using SmartFace.Cli.Core.Domain.DataSelector;
using SmartFace.Cli.Core.Domain.DataSelector.Impl;
using SmartFace.Cli.Core.Domain.ImgExport;
using SmartFace.Cli.Core.Domain.Notifications;
using SmartFace.Cli.Core.Domain.WatchlistMember;
using SmartFace.Cli.Core.Domain.WatchlistMember.Impl;
using SmartFace.Cli.Infrastructure.ApiImplementation;
using SmartFace.ODataClient.Default;
using SmartFace.ODataClient.SmartFace.Domain.DataAccess.Models.Core;

namespace SmartFace.Cli.Common.DI
{
    public static class Installer
    {
        public static ServiceProvider Configure(string[] args)
        {
            var basicArgumentSolverApp = new CommandLineApplication<BasicArgumentSolverCmd>(NullConsole.Singleton);
            basicArgumentSolverApp.ConfigureAsArgumentSolver();
            basicArgumentSolverApp.Execute(args);

            var services = new ServiceCollection()
                .AddSingleton(provider =>
                {
                    var cli = new CommandLineApplication<MainCmd>();

                    if (basicArgumentSolverApp.IsShowingInformation)
                    {
                        cli.UnrecognizedArgumentHandling = UnrecognizedArgumentHandling.StopParsingAndCollect;
                    }

                    return cli;
                })
                .AddSingleton<IApiDefinition>(provider => basicArgumentSolverApp.Model)
                .AddSingleton(provider =>
                {
                    var apiDefinition = provider.GetService<IApiDefinition>();
                    if (basicArgumentSolverApp.IsShowingInformation ||
                        string.IsNullOrEmpty(apiDefinition.ApiUrl))
                    {
                        //dummy container
                        return new Container(null);
                    }

                    return new Container(new Uri(apiDefinition.ODataUrl));
                })
                .AddTransient<HttpClient>()
                .AddSingleton<IImageDownloaderFactory, ImageDownloaderFactory>()
                .AddSingleton<IExportFileResolverFactory, ExportFileResolverFactory>()
                .AddTransient<IQueryDataSelector<Face>, FaceODataSelector>()
                .AddTransient<IQueryDataSelector<Camera>, CameraODataSelector>()
                .AddTransient<IQueryDataSelector<GroupingMetadata>, GroupingODataSelector>()
                .AddTransient<IQueryDataSelector<Individual>, IndividualODataSelector>()
                .AddTransient<IQueryDataSelector<Frame>, FrameODataSelector>()
                .AddTransient<IQueryDataSelector<Scope>, ScopeODataSelector>()
                .AddTransient<IQueryDataSelector<Watchlist>, WatchlistODataSelector>()
                .AddTransient<IQueryDataSelector<MatchResult>, MatchResultODataSelector>()
                .AddTransient<IQueryDataSelector<WatchlistMember>, WatchlistMemberODataSelector>()
                .AddTransient<IQueryDataSelector<Pedestrian>, PedestrianODataSelector>()
                .AddTransient<ICamerasRepository, CamerasRepository>()
                .AddTransient<IWatchlistMembersRepository, WatchlistMembersRepository>()
                .AddTransient<ZeroMqNotificationReader>()
                .AddTransient<WatchlistMemberRegistrationDataJsonLoader>()
                .AddTransient<INotificationReceiver, ZeroMqNotificationReceiver>()
                .AddTransient<IWatchlistMemberRegistrationManager, WatchlistMemberRegistrationManager>()
                .AddLogging(loggingBuilder =>
                {
                    loggingBuilder.AddConsole();
                })
                .BuildServiceProvider();
            return services;
        }
    }
}