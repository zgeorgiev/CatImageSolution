using Domain.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.APIs
{
    public interface IImageApi
    {
        Task<Stream> Get(ImageFilters? filter);
    }
}
