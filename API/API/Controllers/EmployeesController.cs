using API.Base;
using API.Models;
using API.Repository.Data;
using API.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class EmployeesController : BaseController<Employee, EmployeeRepository, string>
    {
        private readonly EmployeeRepository employee;
        public IConfiguration _configuration;
        public EmployeesController(EmployeeRepository employeeRepository, IConfiguration configuration) : base(employeeRepository)
        {
            this.employee = employeeRepository;
            this._configuration = configuration;
        }

        [Route("Register")]
        [HttpPost]
        public ActionResult Register(RegisterVM registerVM)
        {
            var result = employee.Register(registerVM);
            if (result == 1)
            {
                return BadRequest(new { status = HttpStatusCode.BadRequest, message = "NIK sudah tersedia" });
            }
            else if(result == 2)
            {
                return BadRequest(new { status = HttpStatusCode.BadRequest, message = "Email sudah terdaftar" });
            }
            else if (result == 3)
            {
                return BadRequest(new { status = HttpStatusCode.BadRequest, message = "Nomor telepon sudah terdaftar" });
            }
            else
            {
                return Ok(new { HttpStatusCode.OK});
            }
            
        }

        /*[Authorize(Roles = "Director, Manager")]*/
        [HttpGet("Profile")]
        public ActionResult GetProfile()
        {
            var check = employee.CheckData();
            if (check == 0)
            {
                return NotFound(new { status = HttpStatusCode.NotFound, message = "Data tidak ada" });
            }
            else
            {
                var result = employee.GetProfile();
                return Ok(result);
            }
        }

        /*[Authorize(Roles = "Employee")]*/
        [HttpGet("Profile/{NIK}")]
        public ActionResult GetProfile(string NIK)
        {
            try
            {
                var result = employee.GetProfile(NIK);
                return Ok(result);
            }
            catch (InvalidOperationException)
            {
                return NotFound(new { status = HttpStatusCode.NotFound, message = "Data tidak ditemukan" });
            }
        }

        [Route("Login")]
        [HttpPost]
        public ActionResult Login(LoginVM loginVM)
        {
            var result = employee.Login(loginVM);
            if (result == 0)
            {
                var data = new LoginDataVM()
                {
                    Email = loginVM.Email,
                    Roles = employee.GetRole(loginVM)
                };
                var claims = new List<Claim>
                {
                new Claim("email", data.Email)
                };
                foreach (var item in data.Roles)
                {
                    claims.Add(new Claim("roles", item.ToString()));
                }
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                            _configuration["Jwt:Issuer"],
                            _configuration["Jwt:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddMinutes(30),
                            signingCredentials: signIn
                            );
                var idtoken = new JwtSecurityTokenHandler().WriteToken(token);
                claims.Add(new Claim("TokenSecurity", idtoken.ToString()));
                return Ok(new JWTokenVM{ Messages = "Login Berhasil", Token = idtoken });
            }
            else if (result == 1)
            {
                return BadRequest(new JWTokenVM { Messages = "Email/Password Salah", Token = null });
            }
            else 
            {
                return BadRequest(new JWTokenVM { Messages = "Email/Password Salah", Token = null });
            }
        }

        [Authorize(Roles = "Director")]
        [HttpPost("SignManager")]
        public ActionResult SignManager(SignManagerVM signVM)
        {
            var manager = employee.AddAccountRole(signVM);
            return Ok(new { status = HttpStatusCode.OK, message = "Berhasil menambahkan manager" });
        }

        [Route("Gender")]
        [HttpGet]
        public ActionResult Gender()
        {
            var result = employee.CheckGender();
            return Ok(result);
        }

        [Route("Role")]
        [HttpGet]
        public ActionResult Role()
        {
            var result = employee.CheckRole();
            return Ok(result);
        }

        [Route("Salary")]
        [HttpGet]
        public ActionResult Salary()
        {
            var result = employee.CheckSalary1();
            return Ok(result);
        }

        [Route("TestCORS")]
        [HttpGet]
        public ActionResult TestCors()
        {
            return Ok("Test CORS Berhasil");
        }

        [Authorize]
        [HttpGet("TestJWT")]
        public ActionResult TestJwt()
        {
            return Ok("Test JWT Berhasil");
        }
    }
}
