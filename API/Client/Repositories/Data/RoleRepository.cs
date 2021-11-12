using API.Models;
using Client.Base.Urls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Repositories.Data
{
    public class RoleRepository : GeneralRepository<Role, int>
    {
        public RoleRepository(Address address, string request = "Roles/") : base(address, request)
        {

        }
    }
}
