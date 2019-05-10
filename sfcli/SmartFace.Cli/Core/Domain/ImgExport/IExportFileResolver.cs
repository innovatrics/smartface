using System.Collections;
using System.Collections.Generic;

namespace SmartFace.Cli.Core.Domain.ImgExport
{
    public interface IExportFileResolver
    {
        IEnumerable<ExportFile> Resolve(IEnumerable entities);
    }
}