using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartFace.Cli.Core.ApiAbstraction.Models;

namespace SmartFace.Cli.Core.ApiAbstraction
{
    public interface IStreamsRepository
    {
        Task<StreamModel> Get(Guid id);

        Task<IEnumerable<StreamModel>> Get();
    }
}