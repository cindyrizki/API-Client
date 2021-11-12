using API.Models;
using API.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesControllerOld : ControllerBase
    {
        private readonly EmployeeRepository employeeRepository;
        public EmployeesControllerOld(EmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        [HttpPost]
        public ActionResult Post(Employee employee)
        {
            int result = employeeRepository.Insert(employee);
            switch (result)
            {
                case 0: return Ok(new { status = HttpStatusCode.OK, message = "Data berhasil diinput kedalam database", employee });
                case 1: return Ok(new { status = HttpStatusCode.BadRequest, message = "Data gagal dimasukkan, NIK sudah terdata di database" });
                case 2: return Ok(new { status = HttpStatusCode.BadRequest, message = "Data gagal dimasukkan, email sudah terdata di database" });
                case 3: return Ok(new { status = HttpStatusCode.BadRequest, message = "Data gagal dimasukkan, nomor telepon sudah terdata di database" });
            }
            return Ok();
        }

        [HttpGet]
        public ActionResult Get()
        {
            var result = employeeRepository.Get();
            if (result.Count() == 0)
            {
                return Ok(new { status = HttpStatusCode.NoContent, result, message = "Database masih kosong" });
            }
            else
            {
                return Ok(new { status = HttpStatusCode.OK, result, message = "Data berhasil ditammpilkan" });
            }
        }

        [HttpDelete("{NIK}")]
        public ActionResult Delete(string NIK)
        {
            var cek = employeeRepository.Get(NIK);
            if (cek == null)
            {
                return NotFound(new { status = HttpStatusCode.NotFound, message = $"Data tidak ditemukan" });
            }
            else
            {
                var result = employeeRepository.Delete(NIK);
                return Ok(new { status = HttpStatusCode.OK, result, message = $"Data berhasil menghapus data dengan NIK : {NIK}" });
            }
        }

        [HttpPut]
        public ActionResult Update(Employee employee)
        {
            try
            {
                employeeRepository.Update(employee);
                return Ok(new { status = HttpStatusCode.OK, message = $"Berhasil mengubah data {employee.NIK}" });

            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException)
            {
                return NotFound(new { status = HttpStatusCode.NotFound, message = "Data tidak ditemukan" });
            }
        }

        [HttpGet("{NIK}")]
        public ActionResult Get(string NIK)
        {
            var result = employeeRepository.Get(NIK);
            if (result == null)
            {
                return NotFound(new { status = HttpStatusCode.NotFound, result, message = "Data tidak ditemukan" });
            }
            else
            {
                return Ok(new { status = HttpStatusCode.OK, result, message = $"Data berhasil ditammpilkan dengan NIK : {NIK}" });
            }
        }

        [HttpPatch("{NIK}")]
        public ActionResult UpdatePatch(string NIK, Employee employee)
        {
            if (NIK != employeeRepository.Get(employee.NIK).NIK)
            {
                return BadRequest(new { status = HttpStatusCode.OK, message = "Data Gagal Diubah" });
            }
            else
            {
                if (employee.FirstName == null)
                {
                    employee.FirstName = employeeRepository.Get(employee.NIK).FirstName;
                }
                if (employee.LastName == null)
                {
                    employee.LastName = employeeRepository.Get(employee.NIK).LastName;
                }
                if (employee.Email == null)
                {
                    employee.Email = employeeRepository.Get(employee.NIK).Email;
                }
                else
                {
                    foreach (var item in employeeRepository.Get())
                    {
                        if (item.Email == employee.Email)
                        {
                            return Ok(new { status = HttpStatusCode.BadRequest, message = "Data gagal dimasukkan, email sudah terdata di Database" });
                        }
                        else if (item.Phone == employee.Phone)
                        {
                            return Ok(new { status = HttpStatusCode.BadRequest, message = "Data gagal dimasukkan, nomor telepon sudah terdata di Database" });
                        }
                    }
                }
                if (employee.Salary == 0)
                {
                    employee.Salary = employeeRepository.Get(employee.NIK).Salary;
                }
                if (employee.Phone == null)
                {
                    employee.Phone = employeeRepository.Get(employee.NIK).Phone;
                }
                else
                {
                    foreach (var item in employeeRepository.Get())
                    {
                        if (item.Phone == employee.Phone)
                        {
                            return Ok(new { status = HttpStatusCode.BadRequest, message = "Data gagal dimasukkan, nomor telepon sudah terdata di Database" });
                        }
                    }
                }
                if (employee.BirthDate == DateTime.MinValue)
                {
                    employee.BirthDate = employeeRepository.Get(employee.NIK).BirthDate;
                }
                employeeRepository.Update(employee);
                return Ok(new { status = HttpStatusCode.OK, message = "berhasil mengubah data" });
            }
        }
    }
}
