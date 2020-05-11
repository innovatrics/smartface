using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ManagementApi;
using SmartFace.Cli.Common;
using SmartFace.Cli.Core.ApiAbstraction;
using SmartFace.Cli.Core.ApiAbstraction.Models;

namespace SmartFace.Cli.Infrastructure.ApiImplementation
{
    public class CamerasRepository : ICamerasRepository, IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly SetupClient _setupClient;

        public CamerasRepository(IApiDefinition apiDefinition, HttpClient httpClient)
        {
            _httpClient = httpClient;
            _setupClient = new SetupClient(apiDefinition.ApiUrl, _httpClient);
        }

        public async Task<Camera> CreateCameraAsync(CameraRequestData cameraRequestData)
        {
            if (cameraRequestData.Enabled.HasValue && cameraRequestData.Enabled.Value && string.IsNullOrEmpty(cameraRequestData.Source))
            {
                throw new ProcessingException(
                    $"Unable to enable {nameof(CameraRequestData)}, {nameof(cameraRequestData.Source)} is empty");
            }

            var emptyCamera = new Camera
            {
                FaceDetectorConfig = new FaceDetectorConfig()
            };

            var newCameraPayload = UpdateCameraData(emptyCamera, cameraRequestData);
            
            var newCamera = await _setupClient.CamerasPostAsync(newCameraPayload);

            return newCamera;
        }

        public async Task<Camera> UpdateCameraAsync(Guid streamId, CameraRequestData cameraRequestData)
        {
            var originalCamera = await _setupClient.CamerasGetAsync(streamId);

            var updatedCamera = UpdateCameraData(originalCamera, cameraRequestData);

            return await _setupClient.CamerasPutAsync(updatedCamera);
        }

        public Task<Camera> GetCameraAsync(Guid streamId)
        {
            return _setupClient.CamerasGetAsync(streamId);
        }

        public Task<ICollection<Camera>> GetCamerasAsync()
        {
            return _setupClient.CamerasGetAsync();
        }

        private Camera UpdateCameraData(Camera originalCamera, CameraRequestData updateData)
        {
            var updatedCamera = originalCamera;

            updatedCamera.Name = updateData.Name ?? updatedCamera.Name;
            updatedCamera.Source = updateData.Source ?? updatedCamera.Source;
            updatedCamera.Enabled = updateData.Enabled ?? updatedCamera.Enabled;
            updatedCamera.MpeG1PreviewPort = updateData.MPEG1PreviewPort ?? updatedCamera.MpeG1PreviewPort;
            updatedCamera.RedetectionTime = updateData.RedetectionTime ?? updatedCamera.RedetectionTime;

            updatedCamera.FaceDetectorConfig.MaxFaceSize = updateData.TrackMaxFaceSize ?? updatedCamera.FaceDetectorConfig.MaxFaceSize;
            updatedCamera.FaceDetectorConfig.MinFaceSize = updateData.TrackMinFaceSize ?? updatedCamera.FaceDetectorConfig.MinFaceSize;

            //TODO resources
            //updatedCamera.TemplateGeneratorResourceId = updateData.TemplateGeneratorResourceId ?? updatedCamera.TemplateGeneratorResourceId;
            //updatedCamera.FaceDetectorResourceId = updateData.FaceDetectorResourceId ?? updatedCamera.FaceDetectorResourceId;

            return updatedCamera;
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}