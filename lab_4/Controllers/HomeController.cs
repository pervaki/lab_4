using lab_4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

namespace lab_4.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _hostEnvironment;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment hostEnvironment)
        {
            _logger = logger;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
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

        public IActionResult UploadFile()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(FileUploadModel model)
        {
            if (model.FormFile?.Length > 0)
            {
                var filePath = Path.Combine(_hostEnvironment.WebRootPath, "Files", model.FormFile.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.FormFile.CopyToAsync(stream);
                }

                return RedirectToAction("ViewFiles");
            }

            return View();
        }

        public IActionResult ViewFiles()
        {
            var filesDirectory = Path.Combine(_hostEnvironment.WebRootPath, "Files");
            var filePaths = Directory.GetFiles(filesDirectory);
            var fileNames = filePaths.Select(f => Path.GetFileName(f)).ToList();
            return View(fileNames);
        }

        public IActionResult ViewFile(string filename)
        {
            var filePath = Path.Combine(_hostEnvironment.WebRootPath, "Files", filename);
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var fileExtension = Path.GetExtension(filename).ToLower();

            switch (fileExtension)
            {
                case ".jpg":
                case ".jpeg":
                case ".png":
                    return File(fileBytes, "image/jpeg");
                case ".pdf":
                    return File(fileBytes, "application/pdf");
                default: 
                    return File(fileBytes, "application/octet-stream", filename);
            }
        }
    }
}