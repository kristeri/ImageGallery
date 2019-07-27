using ImageGallery.Data;
using ImageGallery.Models;
using ImageGallery.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ImageGallery.Controllers
{
    public class ImageController : Controller
    {
        private IConfiguration _config;
        private IImage _imageservice;
        private string AzureConnectionString { get; set; }

        public ImageController(IConfiguration config, IImage imageService)
        {
            _config = config;
            _imageservice = imageService;
            AzureConnectionString = _config["AzureStorageConnectionString"];
        }

        public IActionResult Upload()
        {
            var model = new UploadImageModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UploadNewImage(IFormFile file, string tags, string title)
        {
            var container = _imageservice.GetBlobContainer(AzureConnectionString, "images");
            var content = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
            var fileName = content.FileName.Trim('"');

            var blockBlob = container.GetBlockBlobReference(fileName);
            await blockBlob.UploadFromStreamAsync(file.OpenReadStream());
            await _imageservice.SetImage(title, tags, blockBlob.Uri);

            return RedirectToAction("Index", "Gallery");
        }
    }
}