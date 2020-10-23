using Microsoft.EntityFrameworkCore;
using ProjectPlutus.Data.Context;
using ProjectPlutus.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPlutus.Infra.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>
    {
        public EmployeeRepository(ProjectPlutusContext context) : base(context)
        {
        }

        public override Employee Update(Employee entity)
        {
            var employee =  _context.Employees
                .SingleOrDefault(e => e.Id == entity.Id);

            employee.FirstName = entity.FirstName;
            employee.LastName = entity.LastName;
            employee.Temperature = entity.Temperature;

            return base.Update(employee);
        }
    }
}
