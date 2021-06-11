using Application.Services;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CatController : ControllerBase
    {
        private readonly IImageService imageService;
        private readonly ILogger<CatController> logger;

        public CatController(IImageService imageService, ILogger<CatController> logger)
        {
            this.imageService = imageService;
            this.logger = logger;
        }

        [HttpGet("flipped")]
        public async Task<IActionResult> GetFlipped(ImageFilters? filter)
        {
            var imageResult = await imageService.Get(filter, RotateFlipType.Rotate180FlipX);
            if (!imageResult.Success) return BadRequest(string.Join(",", imageResult.Errors));

            return File(ImageToByteArray(imageResult.Data), "image/jpeg");
        }

        [HttpGet()]
        public async Task<IActionResult> Get(RotateFlipType? rotationType, ImageFilters? filter)
        {
            var imageResult = await imageService.Get(filter, rotationType);
            if (!imageResult.Success) return BadRequest(string.Join(",", imageResult.Errors));
            
            return File(ImageToByteArray(imageResult.Data), "image/jpeg");
        }

        private byte[] ImageToByteArray(Image imageIn)
        {
            var ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }
    }
}
