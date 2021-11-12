using API.Context;
using API.HashingPassword;
using API.Models;
using API.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository.Data
{
    public class EmployeeRepository : GeneralRepository<MyContext, Employee, string>
    {
        private readonly MyContext context;
        public EmployeeRepository(MyContext myContext) : base(myContext)
        {
			this.context = myContext;
        }

        public int Register(RegisterVM registerVM)
        {
			var checkEmail = context.Employees.Where(p => p.Email == registerVM.Email).FirstOrDefault();
			var checkPhone = context.Employees.Where(p => p.Phone == registerVM.Phone).FirstOrDefault();
			var checkNik = context.Employees.Find(registerVM.NIK);
            if (checkNik != null)
            {
				return 1;
            }
			else if (checkEmail != null)
            {
				return 2;
            }
			else if (checkPhone != null)
            {
				return 3;
            }
            else
            {
                var empResult = new Employee
				{
					NIK = registerVM.NIK,
					FirstName = registerVM.FirstName,
					LastName = registerVM.LastName,
					BirthDate = registerVM.BirthDate,
					Phone = registerVM.Phone,
					Salary = registerVM.Salary,
					Email = registerVM.Email,
					Account = new Account
					{
						NIK = registerVM.NIK,
						Password = Hashing.HashPassword(registerVM.Password),
						Profiling = new Profiling
						{
							NIK = registerVM.NIK,
							Education = new Education
							{
								Degree = registerVM.Degree,
								GPA = registerVM.GPA,
								UniversityId = registerVM.UniversityId
							}
						}
					}
				};

                var empRole = new AccountRole
                {
                    NIK = registerVM.NIK,
                    RoleId = registerVM.RoleId
                };

				context.Employees.Add(empResult);
                context.AccountRoles.Add(empRole);
				var result = context.SaveChanges();
				return result;
            }
		}

        public IEnumerable<ProfileVM> GetProfile()
        {
            var  profile = (from Employee in context.Employees
                            join Account in context.Accounts on Employee.NIK equals Account.NIK
                            join Profiling in context.Profilings on Account.NIK equals Profiling.NIK
                            join Education in context.Educations on Profiling.EducationId equals Education.Id
                            join Universitas in context.Universities on Education.UniversityId equals Universitas.Id
                            join AccountRole in context.AccountRoles on Account.NIK equals AccountRole.NIK
                            select new ProfileVM()
                            {
                                NIK = Employee.NIK,
                                FullName = Employee.FirstName + " " + Employee.LastName,
                                Phone = Employee.Phone,
                                BirthDate = Employee.BirthDate,
                                Salary = Employee.Salary,
                                Email = Employee.Email,
                                Degree = Education.Degree,
                                GPA = Education.GPA,
                                NamaUniversitas = Universitas.Name,
                                AccountRole = AccountRole.RoleId
                             });
            return profile.ToList();
        }

        public Object GetProfile(string NIK)
        {
            var profile = (from Employee in context.Employees
                           join Account in context.Accounts on Employee.NIK equals Account.NIK
                           join Profiling in context.Profilings on Account.NIK equals Profiling.NIK
                           join Education in context.Educations on Profiling.EducationId equals Education.Id
                           join Universitas in context.Universities on Education.UniversityId equals Universitas.Id
                           where Employee.NIK == NIK
                           select new
                           {
                               NIK = Employee.NIK,
                               Fullname = Employee.FirstName + " " + Employee.LastName,
                               Phone = Employee.Phone,
                               BirthDate = Employee.BirthDate,
                               Salary = Employee.Salary,
                               Email = Employee.Email,
                               Degree = Education.Degree,
                               GPA = Education.GPA,
                               Nama_Universitas = Universitas.Name
                           });
            var result = profile.First();
            return result;

        }

        public int CheckData()
        {
            var data = context.Employees.Count();
            if (data > 0)
            {
                return 1;
            }
            return 0;
        }

        public Object CheckGender()
        {
            var data = (from e in context.Employees
                        group e by e.Gender into x
                        select new { gender = x.Key, value = x.Count() });
            return data;
        }

        public Object CheckRole()
        {
            var data = (from e in context.Employees
                        join a in context.Accounts on e.NIK equals a.NIK
                        join b in context.AccountRoles on a.NIK equals b.NIK
                        join r in context.Roles on b.RoleId equals r.Id
                        group r by r.RoleName into x
                        select new { roleName = x.Key, value = x.Count() });
            return data;
        }

        
            

        /*public override int Delete(string NIK)
        {
            var delete = context.Employees.Find(NIK);
            var findProfiling = context.Profilings.Find(NIK);
            var findEdu = context.Educations.Find(findProfiling.EducationId);
            int roleNumbers = context.AccountRoles.Where(p => p.NIK == NIK).Count();
            for (int i = 0; i < roleNumbers; i++)
            {
                var findRoles = context.AccountRoles.Where(p => p.NIK == NIK).FirstOrDefault();
                context.AccountRoles.Remove(findRoles);
                var _result = context.SaveChanges();
            }
            context.Employees.Remove(delete);
            context.Educations.Remove(findEdu);
            var result = context.SaveChanges();
            return result;
        }*/

        public int Login(LoginVM loginVM)
        {
            var checkEmail = context.Employees.Where(p => p.Email == loginVM.Email).FirstOrDefault();

            if (checkEmail == null)
            {
                return 1;
            }
            else
            {
                var dataLogin = checkEmail.NIK;
                var dataPassword = context.Accounts.Find(dataLogin).Password;
                var verify = Hashing.ValidatePassword(loginVM.Password, dataPassword);
                if (verify)
                {
                    return 0;
                }
                else
                {
                    return 2;
                }
            }
        }


        public string[] GetRole(LoginVM loginVM)
        {
            var dataExist = context.Employees.Where(fn => fn.Email == loginVM.Email).FirstOrDefault();
            var userNIK = dataExist.NIK;
            var userRole = context.AccountRoles.Where(fn => fn.NIK == userNIK).ToList();
            List<string> result = new List<string>();
            foreach (var item in userRole)
            {
                result.Add(context.Roles.Where(fn => fn.Id == item.RoleId).First().RoleName);
            }

            return result.ToArray();
        }

        public Object[] CheckSalary1()
        {
            var data = (from e in context.Employees
                        where e.Salary > 2000000
                        select new { 
                            label = ">2000000",
                            value = (from a in context.Employees
                                     where a.Salary > 2000000
                                     select a.Salary).Count()
                        }).First();
            var data1 = (from e in context.Employees
                        where e.Salary <= 2000000 && e.Salary >= 500000
                        select new
                        {
                            label = "500000 - 2000000",
                            value = (from a in context.Employees
                                     where a.Salary <= 2000000 && a.Salary >= 500000
                                     select a.Salary).Count()
                        }).First();
            var data2 = (from e in context.Employees
                         where e.Salary < 500000
                         select new
                         {
                             label = "<500000",
                             value = (from a in context.Employees
                                      where a.Salary < 500000
                                      select a.Salary).Count()
                         }).First();
            List<Object> result = new List<Object>();
            result.Add(data2);
            result.Add(data1);
            result.Add(data);
            return result.ToArray();
        }

        public int AddAccountRole(SignManagerVM signVM)
        {
            var managerRole = new AccountRole
            {
                NIK = signVM.NIK,
                RoleId = signVM.RoleId
            };
            context.AccountRoles.Add(managerRole);
            var result = context.SaveChanges();
            return result;
        }
    }
}
