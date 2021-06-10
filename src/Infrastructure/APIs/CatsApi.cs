using Application.APIs;
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

        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<CatsApi> logger;

        public CatsApi(ILogger<CatsApi> logger, IHttpClientFactory clientFactory)
        {
            this.logger = logger;
            _clientFactory = clientFactory;
        }

        public async Task<Stream> Get(ImageFilters? filter)
        {
            var url = CATS;
            if(filter.HasValue)
            {
                url += "?filter=" + filter.ToString().ToLower();
            }

            try
            {
                var response = await this._clientFactory.CreateClient("cats").GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStreamAsync();
                }
            }
            catch (Exception e)
            {
                this.logger.LogError(e, e.Message);
            }
            return null;
        }
    }
}
