using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ManagementApi;
using SmartFace.Cli.Core.ApiAbstraction;
using SmartFace.Cli.Core.ApiAbstraction.Models;

namespace SmartFace.Cli.Infrastructure.ApiImplementation
{
    public class CamerasRepository : ICamerasRepository, IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly V1Client _setupClient;

        public CamerasRepository(IApiDefinition apiDefinition, HttpClient httpClient)
        {
            _httpClient = httpClient;
            _setupClient = new V1Client(apiDefinition.ApiUrl, _httpClient);
        }

        public async Task<Camera> CreateCameraAsync(CameraRequestData cameraRequestData)
        {
            if (string.IsNullOrEmpty(cameraRequestData.Source))
            {
                throw new ArgumentNullException(nameof(cameraRequestData.Source));
            }

            if (string.IsNullOrEmpty(cameraRequestData.Name))
            {
                throw new ArgumentNullException(nameof(cameraRequestData.Name));
            }

            var newCameraPayload = new CameraCreateRequest
            {
                Name = cameraRequestData.Name,
                Enabled = cameraRequestData.Enabled,
                Source = cameraRequestData.Source,
                MpeG1PreviewPort = cameraRequestData.MPEG1PreviewPort,
                RedetectionTime = cameraRequestData.RedetectionTime,

                FaceDetectorResourceId = cameraRequestData.FaceDetectorResourceId,
                TemplateGeneratorResourceId = cameraRequestData.TemplateGeneratorResourceId,

                FaceDetectorConfig = new FaceDetectorConfigCreateRequest
                {
                    MaxFaceSize = cameraRequestData.TrackMaxFaceSize,
                    MinFaceSize = cameraRequestData.TrackMinFaceSize
                }
            };

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

            updatedCamera.Name = updateData.Name ?? originalCamera.Name;
            updatedCamera.Source = updateData.Source ?? originalCamera.Source;
            updatedCamera.Enabled = updateData.Enabled ?? originalCamera.Enabled;
            updatedCamera.MpeG1PreviewPort = updateData.MPEG1PreviewPort ?? originalCamera.MpeG1PreviewPort;
            updatedCamera.RedetectionTime = updateData.RedetectionTime ?? originalCamera.RedetectionTime;

            updatedCamera.FaceDetectorConfig.MaxFaceSize = updateData.TrackMaxFaceSize ?? originalCamera.FaceDetectorConfig.MaxFaceSize;
            updatedCamera.FaceDetectorConfig.MinFaceSize = updateData.TrackMinFaceSize ?? originalCamera.FaceDetectorConfig.MinFaceSize;

            updatedCamera.TemplateGeneratorResourceId = updateData.TemplateGeneratorResourceId ?? originalCamera.TemplateGeneratorResourceId;
            updatedCamera.FaceDetectorResourceId = updateData.FaceDetectorResourceId ?? originalCamera.FaceDetectorResourceId;

            return updatedCamera;
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}