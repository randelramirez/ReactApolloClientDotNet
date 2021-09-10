using System.Collections.Generic;

namespace Api.GraphQL.Models
{
    public class SessionDtoForJsonData
    {
        public SessionDtoForJsonData()
        {
            this.Speakers = new List<SpeakerDtoForJsonData>();
        }
        
        public string Id { get; set; }

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
        
      public List<SpeakerDtoForJsonData>  Speakers { get; set; }
    }
}