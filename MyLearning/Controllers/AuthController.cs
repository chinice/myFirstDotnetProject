using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyLearning.Dtos;
using MyLearning.Services;
using MyLearning.Utils;

namespace MyLearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly AppSettings _appSettings;

        public AuthController(IAuthRepository authRepository, IOptions<AppSettings> options)
        {
            _authRepository = authRepository;
            _appSettings = options.Value;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, new
                {
                    Status = false,
                    Message = ModelState,
                    Data = new { }
                });
            }

            string hashPassword = HashUtil.HashString(loginDto.Password, "SHA1");
            var user = await _authRepository.Authenticate(loginDto.Username, hashPassword);
            
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddSeconds(int.Parse(_appSettings.TokenExpiry)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            
            if (user == null)
            {
                return Unauthorized(new
                {
                    Status = false,
                    Message = "Invalid credential",
                    Data = new { }
                });
            }
            else
            {
                return Ok(new
                {
                    Status = true,
                    Message = "Login Successful",
                    Data = new
                    {
                        Name = user.Name,
                        Phone = user.Phone,
                        Token = tokenString
                    }
                });
            }
        }
        
    }
}