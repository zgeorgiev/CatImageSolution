using Application.APIs;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTests
{
    public class ImageApiTests : IClassFixture<DependencySetupFixture>
    {
        private ServiceProvider _serviceProvide;
        private IImageApi _imageApi;

        public ImageApiTests(DependencySetupFixture fixture)
        {
            _serviceProvide = fixture.ServiceProvider;
            var scope = _serviceProvide.CreateScope();
            _imageApi = scope.ServiceProvider.GetService<IImageApi>();
        }

        [Fact]
        public async Task Get_Image_Without_Filter()
        {
            var imageRequest = await _imageApi.Get(null);
            imageRequest.Errors.Should().BeEmpty();
            imageRequest.Data.Length.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task Get_Image_With_Filter()
        {
            var imageRequest = await _imageApi.Get(Domain.Enums.ImageFilters.BLUR);
            imageRequest.Errors.Should().BeEmpty();
            imageRequest.Data.Length.Should().BeGreaterThan(0);
        }
    }
}
