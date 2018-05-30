using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.DAL;
using WebApp.Entities;

namespace WebApp.BLL
{
    public static class AdministratorBLL
    {
        public static IEnumerable<Administrator> GetAll()
        {
            try
            {
                AdministratorRepository _AdministratorRepository = new AdministratorRepository();

                return _AdministratorRepository.GetAll<Administrator>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Add(Administrator _Administrator)
        {
            try
            {
                AdministratorRepository _AdministratorRepository = new AdministratorRepository();

                _AdministratorRepository.Add(_Administrator);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
