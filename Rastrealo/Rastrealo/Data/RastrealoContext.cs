using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rastrealo.Models;

namespace Rastrealo.Data
{
    public class RastrealoContext : DbContext
    {
        public RastrealoContext (DbContextOptions<RastrealoContext> options)
            : base(options)
        {
        }

        public DbSet<Rastrealo.Models.admin> admin { get; set; } = default!;

        public DbSet<Rastrealo.Models.bus>? bus { get; set; }

        public DbSet<Rastrealo.Models.driver>? driver { get; set; }

        public DbSet<Rastrealo.Models.route>? route { get; set; }

        public DbSet<Rastrealo.Models.user>? user { get; set; }

        public DbSet<Rastrealo.Models.shift>? shift { get; set; }
    }
}
