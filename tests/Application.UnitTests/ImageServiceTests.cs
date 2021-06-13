using Application.Services;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTests
{
    public class ImageServiceTests : IClassFixture<DependencySetupFixture>
    {
        private ServiceProvider _serviceProvide;
        private IImageService _imageService;

        public ImageServiceTests(DependencySetupFixture fixture)
        {
            _serviceProvide = fixture.ServiceProvider;
            var scope = _serviceProvide.CreateScope();
            _imageService = scope.ServiceProvider.GetService<IImageService>();
        }

        [Fact]
        public async Task Get_Image()
        {
            var imageRequest = await _imageService.Get();
            imageRequest.Errors.Should().BeEmpty();
            imageRequest.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task Get_Image_With_Filter()
        {
            var imageRequest = await _imageService.Get(Domain.Enums.ImageFilters.BLUR);
            imageRequest.Errors.Should().BeEmpty();
            imageRequest.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task Get_Image_With_Filter_And_Rotation()
        {
            var imageRequest = await _imageService.Get(Domain.Enums.ImageFilters.BLUR, System.Drawing.RotateFlipType.RotateNoneFlipX);
            imageRequest.Errors.Should().BeEmpty();
            imageRequest.Data.Should().NotBeNull();
        }
    }
}
