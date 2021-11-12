using Client.Repositories.Data;
using Microsoft.AspNetCore.Mvc;
using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Base.Controllers;
using API.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace Client.Controllers
{
    public class EmployeesController : BaseController<Employee, EmployeeRepository, string>
    {
        private readonly EmployeeRepository employee;
        public EmployeesController(EmployeeRepository repository) : base(repository)
        {
            this.employee = repository;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> GetProfile()
        {
            var result = await employee.GetProfile();
            return Json(result);
        }

        public async Task<JsonResult> Profile(string id)
        {
            var result = await employee.Profile(id);
            return Json(result);
        }

        public JsonResult Register(RegisterVM entity)
        {
            var result = employee.Register(entity);
            return Json(result);
        }

        /*[ValidateAntiForgeryToken]*/
        /*[HttpPost("Login/")]*/
        public async Task<IActionResult> Login(LoginVM login)
        {
            var jwtToken = await employee.Login(login);
            var token = jwtToken.Token;

            if (token == null)
            {
                
                return RedirectToAction("Index", "Home");
            }

            HttpContext.Session.SetString("JWToken", token);

            return RedirectToAction("Dashboard", "Home");
        }

        [Authorize]
       /* [HttpGet("Logout")]*/
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
