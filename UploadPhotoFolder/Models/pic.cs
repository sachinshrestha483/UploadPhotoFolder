using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UploadPhotoFolder.Models
{
    public class pic
    {
        public int id { get; set; }
        [Required]
        public string Address { get; set; }
    }
}
