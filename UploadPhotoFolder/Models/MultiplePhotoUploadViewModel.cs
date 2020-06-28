using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UploadPhotoFolder.Models
{
    public class MultiplePhotoUploadViewModel
    {
        public MultiplePhotoUploadViewModel()
        {
            Photos = new List<IFormFile>();
        }
        public List<IFormFile> Photos { get; set; }

    }
}
