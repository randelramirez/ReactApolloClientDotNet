using System;
using System.Collections.Generic;
using Api.GraphQL.Models;

namespace Api.GraphQL
{
    public class SpeakerComparer : IEqualityComparer<Speaker>
    {
        public bool Equals(Speaker speaker1, Speaker speaker2)
        {
            if (speaker1 == null) throw new ArgumentNullException(nameof(speaker1));
            if (speaker2 == null) throw new ArgumentNullException(nameof(speaker2));
            
            // return speaker1.Featured == speaker2.Featured && speaker1.Bio == speaker2.Bio &&
            //        speaker1.Id == speaker2.Id && speaker1.Name == speaker2.Name;
            return speaker1.Id == speaker2.Id;
        }

        public int GetHashCode(Speaker obj)
        {
            // to do: check if this is correct, otherwise change what needs to be return
            return (int)obj.GetHashCode();
        }
    }
}

