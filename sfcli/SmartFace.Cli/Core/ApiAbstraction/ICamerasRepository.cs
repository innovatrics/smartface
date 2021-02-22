using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ManagementApi;
using SmartFace.Cli.Core.ApiAbstraction.Models;

namespace SmartFace.Cli.Core.ApiAbstraction
{
    public interface ICamerasRepository
    {
        Task<Camera> CreateCameraAsync(CameraRequestData cameraRequestData);
        Task<Camera> UpdateCameraAsync(Guid streamId, CameraRequestData cameraRequestData);
        Task<Camera> GetCameraAsync(Guid streamId);
        Task DeleteCameraAsync(Guid streamId);
        Task<ICollection<Camera>> GetCamerasAsync();
    }
}