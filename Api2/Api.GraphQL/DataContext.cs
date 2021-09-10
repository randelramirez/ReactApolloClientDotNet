using Api.GraphQL.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.GraphQL
{
    public class DataContext : DbContext
    {
        public DbSet<Speaker> Speakers { get; set; }

        public DbSet<Session> Sessions { get; set; }
    }
}