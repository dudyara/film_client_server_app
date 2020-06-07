using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Server
{
    class AnimalContext : DbContext
    {
        public AnimalContext() : base("DBConnection")
        { }
        public DbSet<Films> Animals { get; set; }
    }
}
