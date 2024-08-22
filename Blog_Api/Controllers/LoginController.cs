using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DataLayer.Model_Login;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Text;
using DataLayer.Model_Kullanicilar;
using System;
using DataLayer.Model_DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Blog_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        //mvc den alsın, db ye baksın ok ise user rolünü dönsün. Role göre de mvc kontrolcülere gidebilir.
        private IConfiguration configuration;
        private readonly Identity_DB _context;
        private readonly Blog_DB _ctxt;

        public LoginController(IConfiguration _conf, Identity_DB _DB, Blog_DB ctxt)
        {
            this.configuration = _conf;
            _context = _DB;
            _ctxt = ctxt;
        }

        [HttpPost] //modeli json a serialize etmek gerekli ki mvcde çalışsın!!
        [Route("LoginUser")]
        public IActionResult Login(Login user)
        {
            IActionResult response;
            var IsUser = ControlUser(user.Email, user.Password); //kullanıcılık kontrolü !!
            if (IsUser)
            {
                var tokenStr = CreateJsonWebToken(user); //token oluşturma şeyi bizden geldi !!
                response = Ok(new { token = tokenStr });
                //response = Ok();
            }
            else
                response = Unauthorized("Kullanıcı Oluşturulmamış yada Admin Onayında !!");

            return response;
        }

        [HttpPost] //modeli json a serialize etmek gerekli ki mvcde çalışsın!!
        [Route("Create")]
        public IActionResult Create(Login user)
        {
            IActionResult response;
            var IsUser = ControlUser(user.Email, user.Password); //kullanıcılık kontrolü !!
            if (IsUser)
            {

                response = Unauthorized("Kullanıcı Oluşturulmuş !!");
            }
            else 
            {

                //_context.Users.Add(user);
                response = Ok("Admine Saldık");
            }
                

            return response;
        }
        private bool ControlUser(string userName, string password) //User Kontrolü buradan sağlanıyor !!
        {
            bool res = false;
            bool userExists = _context.Users.Any(u => u.Email == userName);
            if (userExists)
            {
                res = true;
            }
            return res;
        }
        private object CreateJsonWebToken(Login user)
        {

            var users = _context.Users.Where(y => y.Email == user.Email);

            var klaim = new[]
            {
                 //new Claim(JwtRegisteredClaimNames.Sub, "example_user"),
                 //new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                 //claims.Add(new Claim(ClaimTypes.Name, userInfo.UserName));

            new Claim(JwtRegisteredClaimNames.Email,user.Email),
            new Claim("isAdmin", "true", ClaimValueTypes.Boolean) // Bool claim ekleme
            };
            //var claims = new List<Claim>();
            //{
            //    //claims.Add(new Claim(ClaimTypes.Name, userInfo.UserName));

            //    claims.Add(new Claim(ClaimTypes.Email, userInfo.Email));
            //    claims.Add(new Claim("IsAdmin", "false"));
            //    claims.Add(new Claim("IsYazar", "false"));
            //    claims.Add(new Claim("IsKullanici", "false"));
            //}
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(configuration["JWT:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor

            {
                Subject = new ClaimsIdentity(klaim),

                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = configuration["JWT:Issuer"],
                Audience = configuration["JWT:Issuer"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

    }
}

