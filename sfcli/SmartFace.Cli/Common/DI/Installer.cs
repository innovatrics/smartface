using System;
using AutoMapper;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SmartFace.Cli.Commands;
using SmartFace.Cli.Common.DI.Factories;
using SmartFace.Cli.Common.Utils;
using SmartFace.Cli.Core.ApiAbstraction;
using SmartFace.Cli.Core.ApiAbstraction.Models.Configs;
using SmartFace.Cli.Core.Domain.DataSelector;
using SmartFace.Cli.Core.Domain.DataSelector.Impl;
using SmartFace.Cli.Core.Domain.ImgExport;
using SmartFace.Cli.Core.Domain.Notifications;
using SmartFace.Cli.Core.Domain.StreamProcessor;
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
                        string.IsNullOrEmpty(apiDefinition.Host))
                    {
                        //dummy container
                        return new Container(null);
                    }

                    return new Container(new Uri(apiDefinition.ODataUrl));
                })
                .AddSingleton<IMapper, Mapper>(serviceProvider => ConfigureAutoMapper())
                .AddSingleton<IImageDownloaderFactory, ImageDownloaderFactory>()
                .AddSingleton<IExportFileResolverFactory, ExportFileResolverFactory>()
                .AddTransient<IQueryDataSelector<Face>, FaceODataSelector>()
                .AddTransient<IQueryDataSelector<Camera>, CameraODataSelector>()
                .AddTransient<IQueryDataSelector<Grouping>, GroupingODataSelector>()
                .AddTransient<IQueryDataSelector<Individual>, IndividualODataSelector>()
                .AddTransient<IQueryDataSelector<Frame>, FrameODataSelector>()
                .AddTransient<IQueryDataSelector<Scope>, ScopeODataSelector>()
                .AddTransient<IQueryDataSelector<Watchlist>, WatchlistODataSelector>()
                .AddTransient<IQueryDataSelector<MatchResult>, MatchResultODataSelector>()
                .AddTransient<IQueryDataSelector<WatchlistMember>, WatchlistMemberODataSelector>()
                .AddTransient<IVideoProcessorRepository, VideoProcessorRepository>()
                .AddTransient<IWatchlistMembersRepository, WatchlistMembersRepository>()
                .AddTransient<IWorkersRepository, WorkersRepository>()
                .AddTransient<ICamerasRepository, CamerasRepository>()
                .AddTransient<IStreamsRepository, StreamsRepository>()
                .AddTransient<IVideoPublishWorkerConfigRepository, VideoPublishWorkerConfigRepository>()
                .AddTransient<IStreamWorkerConfigRepository, StreamWorkerConfigRepository>()
                .AddTransient<IScopesRepository, ScopesRepository>()
                .AddTransient<IApiProvider, ApiProvider>()
                .AddTransient<ZeroMqNotificationReader>()
                .AddTransient<RegisterWatchlistMemberExtendedJsonLoader>()
                .AddTransient<INotificationReceiver, ZeroMqNotificationReceiver>()
                .AddTransient<IWatchlistMemberRegistrationManager, WatchlistMemberRegistrationManager>()
                .AddLogging(configure => configure.AddConsole())
                .BuildServiceProvider();
            return services;
        }

        public static Mapper ConfigureAutoMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<StreamWorkerConfigModel, VideoProcessor>();
                cfg.CreateMap<VideoPublishWorkerConfigModel, VideoProcessor>();
            });
            return new Mapper(config);
        }
    }
}