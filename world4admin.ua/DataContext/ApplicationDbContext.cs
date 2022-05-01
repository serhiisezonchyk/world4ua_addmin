using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using world4admin.ua.Models;

namespace world4admin.ua.DataContext
{
    public class ApplicationDbContext:DbContext
    {
        public string connectionString = "";
        public ApplicationDbContext(string connectionString) : base(nameOrConnectionString: connectionString) {
            this.connectionString = connectionString;
        }

        public virtual DbSet<empClass> Empobj { get; set; }
    }
}