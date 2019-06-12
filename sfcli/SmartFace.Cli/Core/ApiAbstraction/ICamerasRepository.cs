using SmartFace.Cli.Core.ApiAbstraction.Models;

namespace SmartFace.Cli.Core.ApiAbstraction
{
    public interface ICamerasRepository
    {
        CameraModel Create(CreateCameraModel createCameraModel);
    }
}