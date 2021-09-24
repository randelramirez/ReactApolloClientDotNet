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
            var speakersDto = JsonConvert.DeserializeObject<List<SpeakerDtoForJsonData>>(string.Join("", speakersJson));

            var sessionsJsonFile = Path.Combine(Directory.GetCurrentDirectory(), $"Data", "sessions.json");
            var sessionsJson = System.IO.File.ReadAllLines(sessionsJsonFile);
            var sessionsDto = JsonConvert.DeserializeObject<List<SessionDtoForJsonData>>(string.Join("", sessionsJson));

            // initialize data using the json file
            foreach (var sessionDto in sessionsDto)
            {
                var speakersList = new List<SpeakerDtoForJsonData>();
                sessionDto.Speakers.ForEach(s =>
                {
                    var existingSpeaker = speakersDto.Find(speaker => speaker.Id == s.Id);
                    speakersList.Add(existingSpeaker);
                });
                sessionDto.Speakers = speakersList;
            }

            // sanitize data, session dto has an id that can be guid or long,
            // (currently using string), convert to GUID(Create new Guid)
            var sessionsSeedData = new List<Session>();
            foreach (var sessionDto in sessionsDto)
            {
                var sessionModel = new Session()
                {
                    Id = Guid.NewGuid(),
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
                    EndsAt = sessionDto.EndsAt
                };

                sessionsSeedData.Add(sessionModel);
            }

            var speakersSeedData = new List<Speaker>();
            speakersSeedData.AddRange(speakersDto.Select(speaker =>
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
            
            speakersSeedData.ForEach(speaker =>
            {
                var sessionsGraphFromDto = sessionsDto.Select(sessionDto =>
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