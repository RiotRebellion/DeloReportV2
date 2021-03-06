using Delo.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delo.DAL.Context
{
    public class DeloDB : DbContext
    { 

        public DbSet<Person> Persons { get; set; }

        public DeloDB(DbContextOptions<DeloDB> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
