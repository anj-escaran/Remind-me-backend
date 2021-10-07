using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RemindMe.Models;

namespace RemindMe.Models
{
    public class RepositoryDBContext : DbContext
    {
        public RepositoryDBContext(DbContextOptions<RepositoryDBContext> options) : base(options) { }     
        public DbSet<Account> Account { get; set; }
        public DbSet<Passport> Passport { get; set; }
    }
}
