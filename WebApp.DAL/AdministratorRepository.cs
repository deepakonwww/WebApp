using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Entities;

namespace WebApp.DAL
{
    public class AdministratorRepository : MyRepository<Administrator>
    {        
        public AdministratorRepository()
        {
            DbContext DbContext = new AdminDbContext();

            if (DbContext == null)
            {
                throw new ArgumentNullException("Null DbContext");
            }
            this.DbContext = DbContext;
            DbSet = this.DbContext.Set<Administrator>();
        }
    }
}
