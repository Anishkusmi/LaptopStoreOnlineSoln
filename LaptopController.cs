using LaptopStoreOnline.Data;
using LaptopStoreOnline.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LaptopStoreOnline.Controllers
{
    [Authorize]
    public class LaptopController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment enviroment;

        public LaptopController(ApplicationDbContext context, IWebHostEnvironment enviroment)
        {
            this.context = context;
            this.enviroment = enviroment;
        }
        public IActionResult Index(string? searchString)
        {
            var data = context.laptops.ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                var results = context.laptops.Where(u => u.Brand.Contains(searchString));
                return View(results);
            }

            return View(data);
        }
        public IActionResult IndexUser(string? searchString1)
        {
            var data = context.laptops.ToList();
            if (!String.IsNullOrEmpty(searchString1))
            {
                var results = context.laptops.Where(u => u.Brand.Contains(searchString1));
                return View(results);
            }

            return View(data);
        }

        public IActionResult AddLaptop()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddLaptop(Laptop model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string uniqueFileName = UploadImage(model);
                    var data = new Laptop()
                    {
                        Brand = model.Brand,
                        Description = model.Description,
                        Price = model.Price,
                        Path = uniqueFileName
                    };
                    context.laptops.Add(data);
                    context.SaveChanges();
                    TempData["success"] = "Recored Successfully Added";
                    return RedirectToAction("Index");   
                }
                ModelState.AddModelError(string.Empty, "Model Property is not valid");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(model);
        }
        private string UploadImage(Laptop model)
        {
            string uniqueFileName = string.Empty;
            if (model.ImagePath != null)
            {
                string uploadFolder = Path.Combine(enviroment.WebRootPath, "Content/Laptop/");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImagePath.FileName;  
                string filePath = Path.Combine(uploadFolder, uniqueFileName);
                using(var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ImagePath.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
        [HttpGet]
        public IActionResult Edit(int id)
        { 
            var data = context.laptops.Where(e => e.Id == id).SingleOrDefault();
            if (data != null)
            { 
                return View(data); 
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult Edit(Laptop model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var data = context.laptops.Where(e => e.Id == model.Id).SingleOrDefault();
                    string uniqueFileName = string.Empty;
                    if(model.ImagePath != null)
                    {
                        if(data.Path != null)
                        {
                            string filePath = Path.Combine(enviroment.WebRootPath, "Content/Laptop/", data.Path);
                            if(System.IO.File.Exists(filePath))
                            {
                                System.IO.File.Delete(filePath);
                            }
                        }
                        uniqueFileName = UploadImage(model);
                    }
                    data.Brand = model.Brand;  
                    data.Description = model.Description;
                    data.Price = model.Price;
                    if (model.ImagePath != null)
                    {
                        data.Path = uniqueFileName;
                    }
                    context.laptops.Update(data);
                    context.SaveChanges();
                    TempData["success"] = "Record Updated Successfully";
                }
                else
                {
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return RedirectToAction("Index");

        }
        public IActionResult Delete(int id) 
        { 
            if (id == 0)
            {
                return NotFound();
            }
            else
            {
                var data = context.laptops.Where(e => e.Id == id).SingleOrDefault();
                if (data != null)
                {
                    string deleteFromFolder = Path.Combine(enviroment.WebRootPath, "Content/Laptop/");
                    string currentImage = Path.Combine(Directory.GetCurrentDirectory(), deleteFromFolder,
                        data.Path); 
                    if (currentImage != null) 
                    {
                        if(System.IO.File.Exists(currentImage))
                        {
                            System.IO.File.Delete(currentImage);
                        }
                    }
                    context.laptops.Remove(data);
                    context.SaveChanges();
                    TempData["success"] = "Recorded Deleted";
                }
            }
            return RedirectToAction("Index");
        }
        public IActionResult Details(int id)
        {
            if(id == 0)
            {
                return NotFound();
            }        
            var data = context.laptops.Where(e => e.Id == id).SingleOrDefault();
            return View(data);
        }
    }
}
