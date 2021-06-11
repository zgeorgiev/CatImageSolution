using Application.APIs;
using Domain.Common;
using Domain.Enums;
using Microsoft.Extensions.Logging;
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
        private readonly IImageApi api;
        private readonly ILogger<ImageService> logger;

        public ImageService(IImageApi api, ILogger<ImageService> logger)
        {
            this.api = api;
            this.logger = logger;
        }
        public async Task<Result<Image>> Get(ImageFilters? filter = null, RotateFlipType? rotateFlipType = null)
        {
            var imageResult = await api.Get(filter);
            if (!imageResult.Success) return new Result<Image>() { Errors = imageResult.Errors };
            
            try
            {
                var image = Image.FromStream(imageResult.Data);
                if (rotateFlipType.HasValue)
                {
                    image.RotateFlip(rotateFlipType.Value);
                }
                return new Result<Image>(image);
            }
            catch(ArgumentException exc)
            {
                logger.LogError(exc, exc.Message);

                var errorResult = new Result<Image>();
                errorResult.Errors.Add(exc.Message);
                return errorResult;
            }
        }
    }
}
