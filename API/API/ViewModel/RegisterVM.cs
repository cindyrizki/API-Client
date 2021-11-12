using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModel
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "NIK tidak boleh kosong!")]
        public string NIK { get; set; }
        [Required(ErrorMessage = "First name tidak boleh kosong!")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name tidak boleh kosong!")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Nomor telepon tidak boleh kosong!")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Tanggal lahir tidak boleh kosong!")]
        public DateTime BirthDate { get; set; }
        [Required(ErrorMessage = "Gaji tidak boleh kosong!")]
        public int Salary { get; set; }
        [Required(ErrorMessage = "Email tidak boleh kosong!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password tidak boleh kosong!")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Degree tidak boleh kosong!")]
        public string Degree { get; set; }
        [Required(ErrorMessage = "GPA tidak boleh kosong!")]
        public string GPA { get; set; }
        [Required(ErrorMessage = "Id universitas tidak boleh kosong!")]
        public int UniversityId { get; set; }
        [Required]
        public int RoleId { get; set; }
    }
}
