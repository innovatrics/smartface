using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SmartFace.Cli.Core.ApiAbstraction.Models;

namespace SmartFace.Cli.Core.ApiAbstraction
{
    public interface IWlItemsRepository
    {
        void Register(RegisterWlItemData data);
    }
}
