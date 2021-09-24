using Api.GraphQL.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.GraphQL
{
    public class DataContext : DbContext
    {
        public DbSet<Speaker> Speakers { get; set; }

        public DbSet<Session> Sessions { get; set; }
        
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Session>().HasKey(s => s.Id);
            modelBuilder.Entity<Speaker>().HasKey(s => s.Id);
            
            modelBuilder
                .Entity<Session>()
                .HasMany(session => session.Speakers)
                .WithMany(speaker => speaker.Sessions)
                .UsingEntity(entityTypeBuilder => entityTypeBuilder.ToTable("SessionSpeakers"));
        }
    }
}