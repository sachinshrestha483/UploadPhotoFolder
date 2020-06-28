using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Logging;
using UploadPhotoFolder.Data;
using UploadPhotoFolder.Models;

namespace UploadPhotoFolder.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ApplicationDbContext _db;
        public HomeController(ILogger<HomeController> logger, IHostingEnvironment hostingEnvironment,ApplicationDbContext db)
        {
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            
            return View();
        }


        public IActionResult ShowAll()
        {

            var images = _db.pics.ToList();
            return View(images);
        }

        public IActionResult Create()
        {
            var model = new PhotoUploadViewModel();
            return View(model);
        }
        [HttpPost]
        public IActionResult Create(PhotoUploadViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                var isFormatCorrect=SD.IsImage(model.Photo);
                if(isFormatCorrect==true)
                {
                    string uniqueFileName = ProcessUploadedFile(model);
                    var pic = new pic
                    {
                        Address = uniqueFileName,
                    };
                    _db.pics.Add(pic);
                    _db.SaveChanges();

                }
                else
                {
                    ModelState.AddModelError("", "Wrong Format File Uploaded");
                    return View(model);
                }
               

                

                
               
            }

            return RedirectToAction(nameof(Index));
        }







        public IActionResult MultipleCreate()
        {
            var model = new MultiplePhotoUploadViewModel();
            return View(model);
        }
        [HttpPost]
        public IActionResult MultipleCreate(MultiplePhotoUploadViewModel model)
        {
            if (ModelState.IsValid)
            {
                foreach (var item in model.Photos)
                {
                    var isFormatCorrect = SD.IsImage(item);
                    if (isFormatCorrect == true)
                    {
                        string uniqueFileName = MProcessUploadedFile(item);
                        var pic = new pic
                        {
                            Address = uniqueFileName,
                        };
                        _db.pics.Add(pic);
                        _db.SaveChanges();
                    }

                    else
                    {
                        ModelState.AddModelError("", "Wrong Format File Uploaded");
                        return View(model);
                    }
                }
              






            }

            return RedirectToAction(nameof(Index));
        }




        private string MProcessUploadedFile(IFormFile model)
        {
            string uniqueFileName = null;
            if (model != null)
            {
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }


        private string EProcessUploadedFile(IFormFile model)
        {
            string uniqueFileName = null;
            if (model != null)
            {
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }








        private string ProcessUploadedFile(PhotoUploadViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photo != null)
            {
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }

        public IActionResult Edit(int id)
        {
            var obj = _db.pics.FirstOrDefault(t => t.id == id);
            if(obj==null)
            {
                return RedirectToAction(nameof(ShowAll));
            }
            else
            {
                var vm = new PhotoEditViewModel { };
                return View(vm);
            }

          
        }

        [HttpPost]
        public IActionResult Edit(PhotoEditViewModel model)
        {
            if(ModelState.IsValid)
            {
                if (model.Photo==null)
                {
                var obj = _db.pics.FirstOrDefault(d => d.id == model.id);

                    if (obj == null)
                    {
                        return RedirectToAction(nameof(ShowAll));
                    }
                    else
                    {
                        string filePath = Path.Combine(_hostingEnvironment.WebRootPath,
                                   "images", obj.Address);
                        System.IO.File.Delete(filePath);
                    }
                    
                }
                else
                {
                    var obj = _db.pics.FirstOrDefault(d => d.id == model.id);
                    if (obj == null)
                    {
                        return RedirectToAction(nameof(ShowAll));
                    }
                    else
                    {
                        string filePath = Path.Combine(_hostingEnvironment.WebRootPath,
                                   "images", obj.Address);
                        System.IO.File.Delete(filePath);
                    }

                    var isFormatCorrect = SD.IsImage(model.Photo);
                    if (isFormatCorrect == true)
                    {
                        string uniqueFileName = EProcessUploadedFile(model.Photo);
                        obj.Address = uniqueFileName;
                     
                        _db.pics.Update(obj);
                        _db.SaveChanges();

                    }
                    else
                    {
                        ModelState.AddModelError("", "Wrong Format File Uploaded");
                        return View(model);
                    }



                }
            }
            return View();
        }

        
        
        public IActionResult Delete(int id)
        {
            var obj = _db.pics.FirstOrDefault(d => d.id == id);
            if(obj==null)
            {
                return RedirectToAction(nameof(ShowAll));
            }
            else
            {
                string filePath = Path.Combine(_hostingEnvironment.WebRootPath,
                           "images", obj.Address);
                System.IO.File.Delete(filePath);
            }
            return RedirectToAction(nameof(ShowAll));

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
