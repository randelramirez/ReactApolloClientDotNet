using System;
using System.Collections.Generic;

namespace Api.GraphQL.Models
{
    public class Speaker
    {
        public Speaker()
        {
            this.Sessions = new List<Session>();
        }
        
        public Guid Id { get; set; }
        
        public string Bio { get; set; }
        
        public string Name { get; set; }
        
        public bool Featured { get; set; }
        
        public  ICollection<Session> Sessions { get; set; }

    }
}