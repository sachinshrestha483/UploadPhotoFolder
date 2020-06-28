using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UploadPhotoFolder.Models
{
    public class PhotoUploadViewModel
    {
        public IFormFile Photo { get; set; }
    }
}
