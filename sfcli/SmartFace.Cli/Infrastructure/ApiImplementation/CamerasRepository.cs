using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ManagementApi;
using SmartFace.Cli.Common;
using SmartFace.Cli.Core.ApiAbstraction;
using SmartFace.Cli.Core.ApiAbstraction.Models;
using SmartFace.Cli.Core.Domain;

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

            //TODO when the API takes the resources as string, maybe just forward them, no mapping needed
            updatedCamera.TemplateGeneratorResourceId = MapTemplateResourceId(updateData.TemplateGeneratorResourceId) ?? updatedCamera.TemplateGeneratorResourceId;
            updatedCamera.FaceDetectorResourceId = MapFaceDetectorResourceId(updateData.FaceDetectorResourceId) ?? updatedCamera.FaceDetectorResourceId;

            return updatedCamera;
        }

        private static FaceDetectorResource? MapFaceDetectorResourceId(string resourceId)
        {
            if (string.IsNullOrEmpty(resourceId))
            {
                return null;
            }

            return resourceId switch
            {
                ResourceIds.FaceDetector.ACCURATE_CPU => FaceDetectorResource.AccurateCpu,
                ResourceIds.FaceDetector.ACCURATE_GPU => FaceDetectorResource.AccurateGpu,
                ResourceIds.FaceDetector.BALANCED_CPU => FaceDetectorResource.BalancedCpu,
                ResourceIds.FaceDetector.BALANCED_GPU => FaceDetectorResource.BalancedGpu,
                ResourceIds.FaceDetector.FAST => FaceDetectorResource.Fast,
                _ => throw new ProcessingException($"{resourceId} is not a valid {nameof(ResourceIds.FaceDetector)} resource")
            };
        }

        private static TemplateGeneratorResource? MapTemplateResourceId(string resourceId)
        {
            if (string.IsNullOrEmpty(resourceId))
            {
                return null;
            }

            return resourceId switch
            {
                ResourceIds.TemplateGenerator.WILD_CPU => TemplateGeneratorResource.WildCpu,
                ResourceIds.TemplateGenerator.WILD_GPU => TemplateGeneratorResource.WildGpu,
                _ => throw new ProcessingException($"{resourceId} is not a valid {nameof(ResourceIds.TemplateGenerator)} resource")
            };
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}