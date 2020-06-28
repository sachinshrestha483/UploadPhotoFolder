using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UploadPhotoFolder.Models
{
    public class PhotoEditViewModel
    {
        public int id { get; set; }
        public IFormFile Photo { get; set; }

    }
}
