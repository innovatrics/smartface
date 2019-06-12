using NSubstitute;
using SmartFace.Cli.Core.ApiAbstraction;
using SmartFace.Cli.Core.ApiAbstraction.Models;
using SmartFace.Cli.Infrastructure.ApiImplementation;

namespace Tests
{
    public static class Utils
    {
        public static IApiProvider SubstituteApiProvider()
        {
            return new ApiProvider(null, Substitute.For<IWorkersRepository>(), Substitute.For<IStreamsRepository>(),
                Substitute.For<IScopesRepository>(), Substitute.For<ICamerasRepository>(),
                Substitute.For<IStreamWorkerConfigRepository>(), Substitute.For<IVideoPublishWorkerConfigRepository>(),
                Substitute.For<IFaceHandlerConfigRepository>(), Substitute.For<IIFaceConfigRepository>());
        }
    }
}