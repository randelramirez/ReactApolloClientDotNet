using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Api.GraphQL.Models;
using Microsoft.EntityFrameworkCore;
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
                    Name = sessionFromJSON.Name,
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
            context.SaveChanges();
            
            // add sessions to speakers
            speakersSeedData.ForEach(speaker =>
            {
                var sessionsGraphFromDto = sessionsFromJSON.Select(sessionDto =>
                    new
                    {
                        Id = sessionDto.Id,
                        Day = sessionDto.Day,
                        Description = sessionDto.Description,
                        Favorite = sessionDto.Favorite,
                        Format = sessionDto.Format,
                        Level = sessionDto.Level,
                        Name = sessionDto.Name,
                        Room = sessionDto.Room,
                        Title = sessionDto.Title,
                        Track = sessionDto.Track,
                        StartsAt = sessionDto.StartsAt,
                        EndsAt = sessionDto.EndsAt,
                        Speakers = sessionDto.Speakers.Select(speakerDto =>
                            new Speaker()
                            {
                                Id = speaker.Id,
                                Bio = speaker.Bio,
                                Featured = speaker.Featured,
                                Name = speaker.Name
                            }).ToList()
                    });

                sessionsGraphFromDto.ToList().ForEach(sessionGraph =>
                {
                    if (sessionGraph.Speakers.Any(sessionGraphSpeaker => sessionGraphSpeaker.Id.Equals(speaker.Id)))
                    {
                        var save = sessionsSeedData.Single(
                            sessionSaved => sessionSaved.Title.Equals(sessionGraph.Title));
                        context.Entry(save).State = EntityState.Unchanged;
                        speaker.Sessions.Add(save);
                    }
                });
            });

            context.SaveChanges();
        }
    }
}