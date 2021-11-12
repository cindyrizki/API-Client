using API.Context;
using API.Models;
using API.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly MyContext context;
        public EmployeeRepository(MyContext context)
        {
            this.context = context;
        }
        public int Delete(string NIK)
        {
            var entity = context.Employees.Find(NIK);
            context.Remove(entity);
            var result = context.SaveChanges();
            return result;
        }
        public IEnumerable<Employee> Get()
        {
            return context.Employees.ToList();
        }
        public Employee Get(string NIK)
        {
            var entity = context.Employees.Find(NIK);
            return entity;
        }
        public int Insert(Employee employee)
        {
            var checkEmail = context.Employees.Where(p => p.Email == employee.Email).FirstOrDefault();
            var checkPhone = context.Employees.Where(p => p.Phone == employee.Phone).FirstOrDefault();
            var checkNik = context.Employees.Find(employee.NIK);
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
                context.Employees.Add(employee);
                context.SaveChanges();
                return 0;
            }
        }
        public int Update(Employee employee)
        {
            context.Entry(employee).State = EntityState.Modified;
            var result = context.SaveChanges();
            return result;
        }
    }
}
