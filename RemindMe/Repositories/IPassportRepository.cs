using RemindMe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemindMe.Repositories
{
    public interface IPassportRepository
    {
        Task<Passport> AddPassport(Passport passport);
        Task<Passport> UpdatePassport(Passport passport);
        Task<bool> DeletePassport(int id);
        Task<IEnumerable<Passport>> GetPassports();
        Task<Passport> GetPassport(int id);
    }
}
