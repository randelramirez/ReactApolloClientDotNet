using Api.GraphQL.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.GraphQL
{
    public class DataContext : DbContext
    {
        public DbSet<SpeakerDtoForJsonData> Speakers { get; set; }

        public DbSet<SessionDtoForJsonData> Sessions { get; set; }
        
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Session>().HasKey(s => s.Id);
            modelBuilder.Entity<Session>().HasMany(s => s.Speakers);
            
            modelBuilder.Entity<Speaker>().HasKey(s => s.Id);
            modelBuilder.Entity<Speaker>().HasMany(s => s.Sessions);
        }
    }
}