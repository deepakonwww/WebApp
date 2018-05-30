using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Entities;

namespace WebApp.DAL
{
    public partial class AdminDbContext : DbContext
    {
        public AdminDbContext(): base("name=DBConn")
        {
            Database.SetInitializer<AdminDbContext>(null);
        }

        public virtual DbSet<Administrator> Administrators { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Administrator>().ToTable("tbl_Administrators");

            modelBuilder.Entity<Administrator>().HasKey(t => t.ID);

            /*
            // TableNameConvention
            modelBuilder.Types()
                        .Configure(entity =>
                                   entity.ToTable("tbl_" + entity.ClrType.Name));
                                   */
            base.OnModelCreating(modelBuilder);
        }
    }
}
