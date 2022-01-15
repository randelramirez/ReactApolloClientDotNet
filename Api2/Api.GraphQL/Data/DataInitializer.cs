using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Api.GraphQL.Models;
using Newtonsoft.Json;

namespace Api.GraphQL.Data
{
    public class DataInitializer
    {
        private readonly DataContext context;

        public DataInitializer(DataContext context)
        {
            this.context = context;
        }

        public void SeedDatabase()
        {
            var speakersJsonFile = Path.Combine(Directory.GetCurrentDirectory(), $"Data", "speakers.json");
            var speakersJson = System.IO.File.ReadAllLines(speakersJsonFile);
            var speakersFromJSON = JsonConvert.DeserializeObject<List<SpeakerFromJSON>>(string.Join("", speakersJson));

            var sessionsJsonFile = Path.Combine(Directory.GetCurrentDirectory(), $"Data", "sessions.json");
            var sessionsJson = System.IO.File.ReadAllLines(sessionsJsonFile);
            var sessionsFromJSON = JsonConvert.DeserializeObject<List<SessionFromJSON>>(string.Join("", sessionsJson));

            // session title and speaker name are unique
            // map the speaker and sessions
            var speakerSessionMapping = new Dictionary<string, string>();
            sessionsFromJSON.ForEach(session =>
            {
               
                if (session.Speakers.Count > 0)
                {
                    var speaker = session.Speakers[0]?.Name;
                    speakerSessionMapping.Add(session.Title, speaker);
                }
                else 
                {
                    speakerSessionMapping.Add(session.Title, null);
                }
               
                
            });

            // initialize data using the json file
            // get speakers from the sessions 
            foreach (var sessionFromJSON in sessionsFromJSON)
            {
                var speakers = new List<SpeakerFromJSON>();
                sessionFromJSON.Speakers.ForEach(sessionSpeaker =>
                {
                    var existingSpeaker = speakersFromJSON.Find(speaker => speaker.Id == sessionSpeaker.Id);
                    speakers.Add(existingSpeaker);
                });
                sessionFromJSON.Speakers = speakers;
            }

            // sanitize data, session dto has an id that can be guid or long,
            // (currently using string), convert to GUID(Create new Guid)
            var sessionsSeedData = new List<Session>();
            foreach (var sessionFromJSON in sessionsFromJSON)
            {
                var sessionModel = new Session()
                {
                    Id = Guid.NewGuid(),
                    Day = sessionFromJSON.Day,
                    Description = sessionFromJSON.Description,
                    Favorite = sessionFromJSON.Favorite,
                    Format = sessionFromJSON.Format,
                    Level = sessionFromJSON.Level,
                    Room = sessionFromJSON.Room,
                    Title = sessionFromJSON.Title,
                    Track = sessionFromJSON.Track,
                    StartsAt = sessionFromJSON.StartsAt,
                    EndsAt = sessionFromJSON.EndsAt
                };

                sessionsSeedData.Add(sessionModel);
            }

            var speakersSeedData = new List<Speaker>();
            speakersSeedData.AddRange(speakersFromJSON.Select(speaker =>
                new Speaker()
                {
                    Id = speaker.Id,
                    Bio = speaker.Bio,
                    Featured = speaker.Featured,
                    Name = speaker.Name
                }).ToList());

            speakersSeedData.ForEach(speaker => context.Add(speaker));
            sessionsSeedData.ForEach(session => context.Add(session));

            // map session and speakers
            speakerSessionMapping.ToList().ForEach(mapping =>
            {
                var session = sessionsSeedData.First(session => session.Title == mapping.Key);
                if(mapping.Value != null)
                {
                    var speaker = speakersSeedData.First(speaker => speaker.Name == mapping.Value);
                    session.Speakers.Add(speaker);
                }
              
            });

            context.SaveChanges();
        }
    }
}