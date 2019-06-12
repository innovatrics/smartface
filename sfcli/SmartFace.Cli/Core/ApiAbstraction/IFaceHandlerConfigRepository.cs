using SmartFace.Cli.Core.ApiAbstraction.Models.Configs;

namespace SmartFace.Cli.Core.ApiAbstraction
{
    public interface IFaceHandlerConfigRepository
    {
        FaceHandlerConfigModel Get();

        void Set(FaceHandlerConfigModel configModel);
    }
}
