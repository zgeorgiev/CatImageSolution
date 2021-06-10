using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Domain.Enums;

namespace Application.Services
{
    public interface IImageService
    {
        Task<Image> Get(ImageFilters? filter = null, RotateFlipType? rotateFlipType = null);
    }
}
