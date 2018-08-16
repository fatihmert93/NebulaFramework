using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Nebula.CoreLibrary.Shared
{
    public interface IFileUploader
    {
        string UploadFile(IFormFile formFile);
    }
}
