using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Newtonsoft.Json;
using SmartFace.Cli.Common;
using SmartFace.Cli.Core.Domain.WatchlistMember;
using SmartFace.Cli.Core.Domain.WatchlistMember.Impl;
using SmartFace.Cli.Core.Domain.WatchlistMember.Model;

namespace SmartFace.Cli.Commands.SubWatchlistMember
{
    [Command(Name = "register", Description = "Register single watchlist member")]
    public class RegisterWatchlistMemberCmd
    {
        private readonly IWatchlistMemberRegistrationManager _registrationManager;

        [Required]
        [Option("-i|--id", "", CommandOptionType.SingleValue)]
        public string Id { get; set; }

        [Required]
        [Option("-w|--watchlistIds", "", CommandOptionType.MultipleValue)]
        public string[] WatchlistIds { get; set; }

        [RegistrationImgExtensionValidator]
        [FileExists]
        [Option("-p|--photos <FILE>", "", CommandOptionType.MultipleValue)]
        public string[] Photos { get; set; }

        [Option("--minFaceSize", "", CommandOptionType.SingleValue)]
        public int MinFaceSize { get; set; } = 25;

        [Option("--maxFaceSize", "", CommandOptionType.SingleValue)]
        public int MaxFaceSize { get; set; } = 400;

        [Option("--faceDetConfidenceThreshold", "", CommandOptionType.SingleValue)]
        public int FaceDetectionConfidenceThreshold { get; set; } = 400;

        [Option("--faceDetResourceId", "", CommandOptionType.SingleValue)]
        public string FaceDetectionResourceId { get; set; } = "cpu";

        [Option("--templateGenResourceId", "", CommandOptionType.SingleValue)]
        public string TemplateGeneratorResourceId { get; set; } = "cpu";

        public RegisterWatchlistMemberCmd(IWatchlistMemberRegistrationManager registrationManager)
        {
            _registrationManager = registrationManager;
        }

        protected virtual async Task<int> OnExecuteAsync(CommandLineApplication app, IConsole console)
        {
            var wlMemberData = new WatchlistMemberMetadata
            {
                Id = Id,
                PhotoFiles = Photos,
                WatchlistIds = WatchlistIds
            };

            var registerParams = new RegisterRequestParams(MinFaceSize, MaxFaceSize, FaceDetectionConfidenceThreshold, FaceDetectionResourceId, TemplateGeneratorResourceId, WatchlistIds);

            var result = await _registrationManager.RegisterWatchlistMemberAsync(new WatchlistMemberRegisterData(wlMemberData, registerParams));

            var resultOutput = JsonConvert.SerializeObject(result, Formatting.Indented);
            console.WriteLine(resultOutput);

            return Constants.EXIT_CODE_OK;
        }

    }
}