using ImageGallery.Data;
using ImageGallery.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Threading.Tasks;

namespace ImageGallery.Services
{
    public class ImageService : IImage
    {
        private readonly ImageGalleryDbContext _context;

        public ImageService(ImageGalleryDbContext context)
        {
            _context = context;
        }

        public IEnumerable<GalleryImage> GetAll()
        {
            return _context.GalleryImages
                .Include(image => image.Tags);
        }

        public GalleryImage GetById(int id)
        {
            return GetAll().Where(img => img.Id == id).First();
        }

        public IEnumerable<GalleryImage> GetWithTag(string tag)
        {
            return GetAll()
                .Where(image => image.Tags
                .Any(t => t.Description == tag));
        }

        public CloudBlobContainer GetBlobContainer(string azureConnectionString, string containerName)
        {
            var storageAccount = CloudStorageAccount.Parse(azureConnectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            return blobClient.GetContainerReference(containerName);
        }

        public async Task SetImage(string title, string tags, Uri uri)
        {
            var image = new GalleryImage
            {
                Title = title,
                Tags = ParseTags(tags),
                Url = uri.AbsoluteUri,
                Created = DateTime.Now
        };

            _context.Add(image);
            await _context.SaveChangesAsync();
        }

        public List<ImageTag> ParseTags(string tags)
        {
            return tags.Split(",").ToList().Select(tag => new ImageTag {
                Description = tag
            }).ToList();
        }
    }
}
