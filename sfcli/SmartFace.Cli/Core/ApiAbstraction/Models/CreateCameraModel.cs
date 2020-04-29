using System;

namespace SmartFace.Cli.Core.ApiAbstraction.Models
{
    public class CreateCameraModel
    {
        public Guid ScopeId { get; set; }
        
        public string VideoSource { get; set; }
    }
}