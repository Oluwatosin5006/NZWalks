using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Data
{
    public class NZWalksDbContext : DbContext
    {
        // create a constructor using ctor + Tab
        public NZWalksDbContext(DbContextOptions<NZWalksDbContext> option) : base(option)
        {

        }

        //Create properties of DbSet types (Each Dbset types is for each class in our Data Model)

        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }
        public DbSet<WalkDifficulty> WalkDifficulties { get; set; }
    }
}
