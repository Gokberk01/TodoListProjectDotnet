using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using todoapi.Dtos;
using todoapi.Models;
using todoapi.Services;
using todoapi.Shared;
using todoapi.Shared.Dtos;

namespace todoapi.Controllers
{
    [Route ("api/[Controller]")]
    [ApiController]
    public class AuthController :Controller
    {
        private readonly IUserService _userService;
        private readonly JwtService _jwtservice;
        private readonly ITokenService _refreshTokenService;

        public AuthController(IUserService userService, JwtService jwtService, ITokenService refreshTokenService)
        {
            _refreshTokenService = refreshTokenService;
            _userService = userService;
            _jwtservice = jwtService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(RegisterDto dto)
        {
            var user = new User
            {
                UserName = dto.UserName,
                UserEmail = dto.UserEmail,
                UserPassword = BCrypt.Net.BCrypt.HashPassword(dto.UserPassword)
            };

            return Created("success", await _userService.CreateAsync(user));
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDto dto)
        {
            var user = await _userService.GetByEmail(dto.UserEmail);

            if(user == null)
            {
                return BadRequest(new {message = "Invalid Credentials"});
            }

            if(!BCrypt.Net.BCrypt.Verify(dto.UserPassword, user.UserPassword))
            {
                return BadRequest(new {message = "Invalid Credentials"});
            }

            int expirationTime = 1;
            var jwt = _jwtservice.Generate(user.UserId, expirationTime);

            var _refreshToken = _refreshTokenService.GenerateRefreshToken();

            var refreshTokenEntity = new RefreshToken
            {
                Token = _refreshToken,
                UserId = user.UserId.ToString(),
                Expires = DateTime.UtcNow.AddDays(3), // 3 gün geçerlilik süresi
                Created = DateTime.UtcNow,
            };
            await _refreshTokenService.AddRefreshToken(refreshTokenEntity);


            //jwt will be inside the httponly cookie where the front end cannot access it. Only purpose of httponly cookie is to
            //get it in the backend and backedn can modify it or access it. The front end just have to get it and send it
            // Response.Cookies.Append("jwt", jwt, new CookieOptions
            // {
            //     HttpOnly = true
            // });

            return Ok(new 
            { 
                token = jwt,
                refreshToken = _refreshToken             
            });
        }


        [HttpPost("logout")] //Delete the created cookie
        public async Task<ActionResult> Logout(LogoutDto refreshToken)
        {
            if (!Request.Headers.TryGetValue("Authorization", out StringValues authHeader))
            {
                return Unauthorized();
            }

            var bearerToken = authHeader.ToString().Split(' ').Last();

            var token = _jwtservice.Verify(bearerToken);

            if(token == null)
            {
                return Unauthorized();
            }

            var userId = token.Issuer;

            var RefreshTokenEntity = await _refreshTokenService.GetRefreshToken(refreshToken.RefreshToken);

            if(RefreshTokenEntity == null || RefreshTokenEntity.UserId != userId || !await _refreshTokenService.IsRefreshTokenValid(refreshToken.RefreshToken))
            {
                return Unauthorized();
            }

            await _refreshTokenService.RevokeRefreshToken(refreshToken.RefreshToken);

            return Ok(new 
            {
                message = "Logout successfull", 
            });

        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult> RefreshToken(TokenRequestDto tokenRequestDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            } 

            var existingRefreshToken = await _refreshTokenService.GetRefreshToken(tokenRequestDto.RefreshToken);

            if(existingRefreshToken == null || !existingRefreshToken.IsActive)
            {
                return Unauthorized(new { message = "Invalid refresh token"});
            }

            var token = _jwtservice.Verify(tokenRequestDto.AccessToken,false); 

            if(token == null)
            {
                return Unauthorized(new { message = "Invalid access token"});
            }

            var userId = token.Issuer;

            if(existingRefreshToken.UserId != userId)
            {
                return Unauthorized(new { message = "Invalid refresh token for this user"});
            }

            int expirationTime = 1;
            var newAccessToken = _jwtservice.Generate(int.Parse(userId),expirationTime);
            var newRefreshToken = _refreshTokenService.GenerateRefreshToken();

            await _refreshTokenService.RevokeRefreshToken(tokenRequestDto.RefreshToken);

            var newRefreshTokenEntity = new RefreshToken
            {
                Token = newRefreshToken,
                UserId = userId,
                Expires = DateTime.UtcNow.AddDays(3), // 3 gün geçerlilik süresi
                Created = DateTime.UtcNow,
            };
            await _refreshTokenService.AddRefreshToken(newRefreshTokenEntity);

            return Ok(new 
            { 
                token = newAccessToken,
                refreshToken = newRefreshToken             
            });
        }

        // [HttpPost("logout")] //Delete the created cookie
        // public async Task<ActionResult<User>> Logout()
        // {
        //     Response.Cookies.Delete("jwt");

        //     return Ok(new 
        //     {
        //         message = "success"
        //     });

        // }
        
    }
}