using Microsoft.EntityFrameworkCore;
using RemindMe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemindMe.Repositories
{
    public class PassportRepository : IPassportRepository
    {
        private readonly RepositoryDBContext _db;
        public PassportRepository(RepositoryDBContext context) => _db = context;

        public async Task<Passport> AddPassport(Passport passport)
        {
            try
            {
                _db.Passport.Add(passport);
                await _db.SaveChangesAsync();

                return passport;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> DeletePassport(int id)
        {
            try
            {
                var passport = _db.Passport.Find(id);
                if (passport != null) _db.Passport.Remove(passport);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Passport> GetPassport(int id) => await _db.Passport.FirstOrDefaultAsync(x => x.Id == id);
        public async Task<IEnumerable<Passport>> GetPassports() => await _db.Passport.ToListAsync();

        public async Task<Passport> UpdatePassport(Passport passport)
        {
            try
            {
                _db.Entry(passport).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return passport;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
