using System;
using System.Collections.Generic;
using System.Text;
using SmartFace.Cli.ApiAbstraction.Models.Configs;

namespace SmartFace.Cli.ApiAbstraction
{
    public interface IFaceHandlerConfigRepository
    {
        FaceHandlerConfigModel Get();

        void Set(FaceHandlerConfigModel configModel);
    }
}
