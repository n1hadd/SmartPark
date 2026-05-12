using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartPark.Models;

namespace SmartPark.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext (DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<SmartPark.Models.Parkirisce> Parkirisce { get; set; } = default!;
        public DbSet<SmartPark.Models.ParkirnoMesto> ParkirnoMesto { get; set; } = default!;
        public DbSet<SmartPark.Models.Rezervacija> Rezervacija { get; set; } = default!;
        public DbSet<SmartPark.Models.Placilo> Placilo { get; set; } = default!;
    }
}
