using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using todoapi.Models;
using todoapi.Services;
using todoapi.Shared;

namespace todoapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly JwtService _jwtservice;

        public UserController(IUserService userService, JwtService jwtService)
        {
            _userService = userService;
            _jwtservice = jwtService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<User>> GetUser()
        {
            try
            {

                // Authorization başlığından JWT token'ı al
                if (!Request.Headers.TryGetValue("Authorization", out StringValues authHeader))
                {
                    return Unauthorized();
                }

                var bearerToken = authHeader.ToString().Split(' ').Last();

                var token = _jwtservice.Verify(bearerToken);

                // Token'dan userId'yi al
                int userId = int.Parse(token.Issuer);

                // UserId'ye göre kullanıcı bilgilerini al
                var user = await _userService.GetById(userId);

                return Ok(user);
            }
             catch (Exception)
            {
                
                return Unauthorized();
            }


        }
    }
}