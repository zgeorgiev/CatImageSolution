using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using System.Net;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text.Json;
using WebApi.DataContracts.Users;
using WebApi.DataContracts;

namespace WebApi.IntegrationTests
{
    public class CatAndUserTests : IntegrationTest
    {
        private readonly string catUrl = "cat";
        private readonly string flippedCatUrl = "cat/flipped";
        private readonly string userUrl = "user";

        [Fact]
        public async Task Get_Flipped_Image()
        {
            Authenticate();
            var response = await this.TestClient.GetAsync(flippedCatUrl);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Headers.ContentLength.Should().BeGreaterThan(0);
            var image = Image.FromStream(response.Content.ReadAsStream());
            image.RawFormat.Should().Be(ImageFormat.Jpeg);
        }

        [Fact]
        public async Task Get_Require_Authorization()
        {
            var response = await this.TestClient.GetAsync(catUrl);
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Get_Image_With_Filter()
        {
            Authenticate();
            var response = await this.TestClient.GetAsync(catUrl + "?filter=SEPIA");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var image = Image.FromStream(response.Content.ReadAsStream());
            image.RawFormat.Should().Be(ImageFormat.Jpeg);
        }

        [Fact]
        public async Task Get_Image_With_Filter_And_Rotation()
        {
            Authenticate();
            var response = await this.TestClient.GetAsync(catUrl + "?filter=SEPIA&rotationType=Rotate180FlipNone");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var image = Image.FromStream(response.Content.ReadAsStream());
            image.RawFormat.Should().Be(ImageFormat.Jpeg);
        }

        [Fact]
        public async Task Get_User_Not_Authorized()
        {
            var response = await this.TestClient.GetAsync(userUrl);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Get_User_Details()
        {
            Authenticate();
            var response = await this.TestClient.GetAsync(userUrl);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            JsonSerializer.Deserialize<UserDetails>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }).Username.Should().Be("demouser");
        }

        [Fact]
        public async Task Post_Register_User()
        {
            var newUsername = Guid.NewGuid().ToString("N");
            var newUser = new UserRegister
            {
                Username = newUsername,
                Password = Guid.NewGuid().ToString("N"),
                Name = "Name" + newUsername
            };
            var content = new StringContent(JsonSerializer.Serialize(newUser), Encoding.UTF8);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = await this.TestClient.PostAsync(userUrl, content);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            JsonSerializer.Deserialize<UserDetails>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }).Username.Should().Be(newUsername);
        }
    }
}
