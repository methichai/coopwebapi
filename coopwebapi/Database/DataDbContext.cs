using coopwebapi.Models;
using Microsoft.EntityFrameworkCore;

namespace coopwebapi.Database
{
    public class DataDbContext:DbContext
    {
        public DataDbContext(DbContextOptions<DataDbContext> options) : base(options) { }
        public DbSet<manufacturers> manufacturers { get; set; }
        public DbSet<devices> devices { get; set; }
    }
}
