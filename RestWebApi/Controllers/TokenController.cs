using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RestWebApi.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RestWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        public IConfiguration _configuration;
        public readonly EmployeeContext _employeeContext;
        public TokenController(IConfiguration configuration, EmployeeContext employeeContext)
        {
            _configuration = configuration;
            _employeeContext = employeeContext;
        }
        [HttpPost]
        public async Task<IActionResult> Post(UserInfo userInfo)
        {
            try
            {
            
            if (userInfo != null && userInfo.UserName != null && userInfo.Password != null)
            {
                var user = await GetUser(userInfo.UserName, userInfo.Password);
                if (user != null)
                {
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub,_configuration["Jwt:Subject"]) ,
                        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()) ,
                        new Claim(JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToString()) ,
                        new Claim("Id",user.UseId.ToString()),
                        new Claim("UserName",user.UserName.ToString()),
                        new Claim("Password",user.Password.ToString()),
                    };


                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signTn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.Now.AddMinutes(20),
                        signingCredentials: signTn);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));

                }
                else
                {
                    return BadRequest("Invalid Credentails");
                }

            }
            else
            {
                return BadRequest();
            }
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        public async Task<UserInfo> GetUser(string userName, string password)
        {
            return await _employeeContext.UserInfo.FirstOrDefaultAsync(u => u.UserName == userName && u.Password == password);
            //throw new NotImplementedException();
        }
    }
}
