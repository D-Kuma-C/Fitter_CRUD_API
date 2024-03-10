using Fitter_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Fitter_API.Data
{
    public class FitterDbContext : DbContext
    {
        public DbSet<Fitter> Fitters { get; set; }
        public DbSet<SeniorFitter> SeniorFitters { get; set; }
        public FitterDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
