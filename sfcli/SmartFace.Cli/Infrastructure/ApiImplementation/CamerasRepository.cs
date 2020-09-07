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
                ObjectDetectorResourceId = cameraRequestData.ObjectDetectorResourceId,

                FaceDetectorConfig = new FaceDetectorConfigCreateRequest
                {
                    MaxFaceSize = cameraRequestData.TrackMaxFaceSize,
                    MinFaceSize = cameraRequestData.TrackMinFaceSize
                },
                ObjectDetectorConfig = new ObjectDetectorConfigCreateRequest
                {
                    MaxObjectSize = cameraRequestData.MaxObjectSize,
                    MinObjectSize = cameraRequestData.MinObjectSize
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

        private CameraUpdateRequest UpdateCameraData(Camera originalCamera, CameraRequestData updateData)
        {
            var updatedCamera = new CameraUpdateRequest
            {
                Id = originalCamera.Id,
                FaceDetectorConfig = originalCamera.FaceDetectorConfig,
                FaceSaveStrategy = originalCamera.FaceSaveStrategy,
                ImageQuality = originalCamera.ImageQuality,
                TrackMotionOptimization = originalCamera.TrackMotionOptimization,
                MaskImagePath = originalCamera.MaskImagePath,
                TemplateGenerationTime = originalCamera.TemplateGenerationTime,
                PreviewMaxDimension = originalCamera.PreviewMaxDimension,
                MpeG1PreviewEnabled = originalCamera.MpeG1PreviewEnabled,
                MpeG1VideoBitrate = originalCamera.MpeG1VideoBitrate,
                SaveFrameImageData = originalCamera.SaveFrameImageData,

                Name = updateData.Name ?? originalCamera.Name,
                Source = updateData.Source ?? originalCamera.Source,
                Enabled = updateData.Enabled ?? originalCamera.Enabled,
                MpeG1PreviewPort = updateData.MPEG1PreviewPort ?? originalCamera.MpeG1PreviewPort,
                RedetectionTime = updateData.RedetectionTime ?? originalCamera.RedetectionTime,
                TemplateGeneratorResourceId =
                    updateData.TemplateGeneratorResourceId ?? originalCamera.TemplateGeneratorResourceId,
                FaceDetectorResourceId = updateData.FaceDetectorResourceId ?? originalCamera.FaceDetectorResourceId,
                ObjectDetectorResourceId = updateData.ObjectDetectorResourceId ?? originalCamera.ObjectDetectorResourceId
            };

            updatedCamera.FaceDetectorConfig.MaxFaceSize = updateData.TrackMaxFaceSize ?? originalCamera.FaceDetectorConfig.MaxFaceSize;
            updatedCamera.FaceDetectorConfig.MinFaceSize = updateData.TrackMinFaceSize ?? originalCamera.FaceDetectorConfig.MinFaceSize;
            
            updatedCamera.ObjectDetectorConfig.MaxObjectSize = updateData.MaxObjectSize ?? originalCamera.ObjectDetectorConfig.MaxObjectSize;
            updatedCamera.ObjectDetectorConfig.MinObjectSize = updateData.MinObjectSize ?? originalCamera.ObjectDetectorConfig.MinObjectSize;

            return updatedCamera;
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}