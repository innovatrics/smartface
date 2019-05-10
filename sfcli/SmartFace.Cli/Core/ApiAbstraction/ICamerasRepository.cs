using SmartFace.Cli.ApiAbstraction.Models;

namespace SmartFace.Cli.ApiAbstraction
{
    public interface ICamerasRepository
    {
        CameraModel Create(CreateCameraModel createCameraModel);
    }
}