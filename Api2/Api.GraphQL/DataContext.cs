using Api.GraphQL.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.GraphQL
{
    public class DataContext : DbContext
    {
        public DbSet<SpeakerDtoForJsonData> Speakers { get; set; }

        public DbSet<SessionDtoForJsonData> Sessions { get; set; }
    }
}