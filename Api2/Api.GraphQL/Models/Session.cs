using System;
using Newtonsoft.Json;

namespace Api.GraphQL.Models
{
    public class Session
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string StartsAt { get; set; }

        public string EndsAt { get; set; }

        public string Room { get; set; }

        public string Day { get; set; }

        public string Format { get; set; }

        public string Track { get; set; }

        public string Level { get; set; }

        public bool Favorite { get; set; }

        [JsonIgnore] public Guid SpeakerId { get; set; }

        [JsonIgnore] public virtual Speaker Speaker { get; set; }
    }
}