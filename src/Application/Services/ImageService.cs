using Application.APIs;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ImageService : IImageService
    {
        private readonly IImageApi _api;
        public ImageService(IImageApi api)
        {
            _api = api;
        }
        public async Task<Image> Get(ImageFilters? filter = null, RotateFlipType? rotateFlipType = null)
        {
            var imageStream = await _api.Get(filter);
            if (imageStream == null) return null;
            var image = Image.FromStream(imageStream);
            if(rotateFlipType.HasValue)
            {
                image.RotateFlip(rotateFlipType.Value);
            }
            return image;
        }
    }
}
