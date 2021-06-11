using Application.APIs;
using Domain.Common;
using Domain.Enums;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.APIs
{
    public class CatsApi : IImageApi
    {
        private static readonly string CATS = "/c";

        private readonly IHttpClientFactory clientFactory;
        private readonly ILogger<CatsApi> logger;

        public CatsApi(ILogger<CatsApi> logger, IHttpClientFactory clientFactory)
        {
            this.logger = logger;
            this.clientFactory = clientFactory;
        }

        public async Task<Result<Stream>> Get(ImageFilters? filter)
        {
            var url = CATS;
            if(filter.HasValue)
            {
                url += "?filter=" + filter.ToString().ToLower();
            }
            Result<Stream> result = new();
            try
            {
                var response = await this.clientFactory.CreateClient("cats").GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    result.Data = await response.Content.ReadAsStreamAsync();
                    return result;
                }
                else
                {
                    this.logger.LogError(response.ReasonPhrase);
                    result.Errors.Add(response.ReasonPhrase);
                }
            }
            catch (Exception e)
            {
                result.Errors.Add(e.Message);
                this.logger.LogError(e, e.Message);
            }
            return result;
        }
    }
}
