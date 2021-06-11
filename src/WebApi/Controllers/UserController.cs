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
using System.Security.Claims;
using System.Threading.Tasks;
using WebApi.DataContracts;
using WebApi.DataContracts.Users.Mappers;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly ILogger<UserController> logger;

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            this.logger = logger;
            this.userService = userService;
        }

        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Guid userId;
            if(!Guid.TryParse(userIdClaim, out userId))
            {
                return BadRequest("No user exist in the system");
            }
            var user = await userService.GetById(userId);
            if(user == null)
            {
                return BadRequest("No user exist in the system");
            }
            return Ok(user);
        }

        [HttpPost()]
        [AllowAnonymous]
        public async Task<IActionResult> Post(UserDataContract model)
        {
            return Ok(await this.userService.RegisterAsync(UserMapper.MapDataContractToEntity(model)));
        }
    }
}
