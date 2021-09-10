using System;

namespace Api.GraphQL.Models
{
    public class SpeakerDtoForJsonData
    {
        public Guid Id { get; set; }
        
        public string Bio { get; set; }
        
        public string Name { get; set; }
        
        public bool Featured { get; set; }
        
    }
}