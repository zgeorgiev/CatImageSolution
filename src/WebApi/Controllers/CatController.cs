using Application.Services;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CatController : ControllerBase
    {
        private readonly IImageService imageService;

        public CatController(IImageService imageService)
        {
            this.imageService = imageService;
        }

        [HttpGet()]
        public async Task<IActionResult> Get(RotateFlipType? rotationType, ImageFilters? filter)
        {
            var image = await imageService.Get(filter, rotationType);
            if (image == null) return BadRequest();
            
            return File(ImageToByteArray(image), "image/jpeg");
        }

        private byte[] ImageToByteArray(Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }
    }
}
