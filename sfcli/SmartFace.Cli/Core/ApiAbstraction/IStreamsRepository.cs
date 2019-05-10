using System.Collections.Generic;
using System.Threading.Tasks;
using SmartFace.Cli.ApiAbstraction.Models;
using SmartFace.ODataClient.SmartFace.Data.Models.Core;

namespace SmartFace.Cli.ApiAbstraction
{
    public interface IStreamsRepository
    {
        Task<StreamModel> Get(long id);

        Task<IEnumerable<StreamModel>> Get();
    }
}