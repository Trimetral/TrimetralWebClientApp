using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ClientsWebApp.Models;

using Microsoft.EntityFrameworkCore;

namespace ClientsWebApp.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> dbContextOptions) : base(dbContextOptions) { }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Founder> Founders { get; set; }

    }
}
