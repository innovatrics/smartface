using System;
using AutoMapper;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SmartFace.Cli.ApiAbstraction;
using SmartFace.Cli.ApiAbstraction.Models;
using SmartFace.Cli.ApiAbstraction.Models.Configs;
using SmartFace.Cli.Commands;
using SmartFace.Cli.Common.DI.Factories;
using SmartFace.Cli.Common.Utils;
using SmartFace.Cli.Core.Domain.DataSelector;
using SmartFace.Cli.Core.Domain.DataSelector.Impl;
using SmartFace.Cli.Core.Domain.GlobalConfig;
using SmartFace.Cli.Core.Domain.ImgExport;
using SmartFace.Cli.Core.Domain.Notifications;
using SmartFace.Cli.Core.Domain.StreamProcessor;
using SmartFace.Cli.Core.Domain.WatchlistItem;
using SmartFace.Cli.Infrastructure.ApiImplementation;
using SmartFace.ODataClient.Default;
using SmartFace.ODataClient.SmartFace.Data.Models.Core;

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
                        cli.ThrowOnUnexpectedArgument = false;
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
                .AddSingleton<IMapper, Mapper>(serviceProvider => ConfigureAutoMapper())
                .AddSingleton<IImageDownloaderFactory, ImageDownloaderFactory>()
                .AddSingleton<IExportFileResolverFactory, ExportFileResolverFactory>()
                .AddTransient<IQueryDataSelector<Face>, FaceODataSelector>()
                .AddTransient<IQueryDataSelector<Camera>, CameraODataSelector>()
                .AddTransient<IQueryDataSelector<Grouping>, GroupingODataSelector>()
                .AddTransient<IQueryDataSelector<Identity>, IdentityODataSelector>()
                .AddTransient<IQueryDataSelector<Photo>, PhotoODataSelector>()
                .AddTransient<IQueryDataSelector<Scope>, ScopeODataSelector>()
                .AddTransient<IQueryDataSelector<Watchlist>, WatchlistODataSelector>()
                .AddTransient<IQueryDataSelector<WlHit>, WlHitODataSelector>()
                .AddTransient<IQueryDataSelector<WlItem>, WlItemODataSelector>()
                .AddTransient<IVideoProcessorRepository, VideoProcessorRepository>()
                .AddTransient<IWlItemRepository, WlItemRepository>()
                .AddTransient<IWorkersRepository, WorkersRepository>()
                .AddTransient<ICamerasRepository, CamerasRepository>()
                .AddTransient<IStreamsRepository, StreamsRepository>()
                .AddTransient<IVideoPublishWorkerConfigRepository, VideoPublishWorkerConfigRepository>()
                .AddTransient<IStreamWorkerConfigRepository, StreamWorkerConfigRepository>()
                .AddTransient<IFaceHandlerConfigRepository, FaceHandlerConfigRepository>()
                .AddTransient<IIFaceConfigRepository, IFaceConfigRepository>()
                .AddTransient<IScopesRepository, ScopesRepository>()
                .AddTransient<IGlobalConfigRepository, GlobalConfigRepository>()
                .AddTransient<IApiProvider, ApiProvider>()
                .AddTransient<ZeroMqNotificationReader>()
                .AddTransient<INotificationReceiver, ZeroMqNotificationReceiver>()
                .AddTransient<IWatchlistItemRegistrationManager, WatchlistItemRegistrationManager>()
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
                cfg.CreateMap<FaceHandlerConfigModel, GlobalConfig>();
                cfg.CreateMap<IFaceConfigModel, GlobalConfig>();
            });
            return new Mapper(config);
        }
    }
}