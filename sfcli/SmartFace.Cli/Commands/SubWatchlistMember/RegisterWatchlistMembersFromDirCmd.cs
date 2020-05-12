using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using SmartFace.Cli.Common;
using SmartFace.Cli.Core.Domain.WatchlistMember;

namespace SmartFace.Cli.Commands.SubWatchlistMember
{
    [Command(Name = "registerFromDir", Description = "Register WatchlistMember entities from photos in directory in format {watchlistmember_externalId}.(jpeg|jpg|png) ")]
    public class RegisterWatchlistMembersFromDirCmd
    {
        private readonly IWatchlistMemberRegistrationManager _registrationManager;
        
        [Required]
        [Option("-w|--watchlistsExternalIds", "", CommandOptionType.MultipleValue)]
        public string[] WatchlistExternalIds { get; set; }
        
        [Required]
        [Option("-d|--dirToPhotos", "", CommandOptionType.SingleValue)]
        public string Directory { get; set; }

        [Option("-m|--metaDataFile", @"Use this option when you can provide single json file in selected directory with meta data for WatchlistMember. In this case could be use any name for photo file
[
{
    ""ExternalId"": ""120"",
    ""DisplayName"": ""Display name"",
    ""FullName"": ""Full name"",
    ""Note"": ""Example note"",
    ""PhotoFiles"": [""file1.jpeg"", ""file2.jpeg""]
} 
]
", CommandOptionType.NoValue)]
        public bool UseMetaDataFile { get; set; }

        [Option("-p|--parallel", "Max degree of parallelism, default value is 1", CommandOptionType.SingleValue)]
        public int MaxDegreeOfParallelism { get; set; } = 1;

        public RegisterWatchlistMembersFromDirCmd(IWatchlistMemberRegistrationManager registrationManager)
        {
            _registrationManager = registrationManager;
        }

        protected virtual async Task<int> OnExecuteAsync(CommandLineApplication app, IConsole console)
        {
            if (UseMetaDataFile)
            {
                await _registrationManager.RegisterWatchlistMembersExtendedFromDirAsync(Directory, WatchlistExternalIds, MaxDegreeOfParallelism);
            }
            else
            {
                await _registrationManager.RegisterWatchlistMembersFromDirAsync(Directory, WatchlistExternalIds, MaxDegreeOfParallelism);
            }
            return Constants.EXIT_CODE_OK;
        }
    }
}
