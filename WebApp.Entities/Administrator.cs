using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Entities
{    
    public class Administrator : IDisposable
    {
        [Key]
        [Column("AdministratorID")]
        public int ID { get; set; }
        public DateTime? RegisterDate { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public DateTime? PasswordChanged { get; set; }
        public int InvalidLoginAttempt { get; set; }
        public DateTime? LockedUntil { get; set; }
        public bool IsActive { get; set; }
        public Guid? SessionID { get; set; }
        public bool IsPowerUser { get; set; }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
