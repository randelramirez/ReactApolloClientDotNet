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
            var speakersDto = JsonConvert.DeserializeObject<List<SpeakerDtoForJsonData>>(string.Join("", speakersJson));

            var sessionsJsonFile = Path.Combine(Directory.GetCurrentDirectory(), $"Data", "sessions.json");
            var sessionsJson = System.IO.File.ReadAllLines(sessionsJsonFile);

            var sessionsDto = JsonConvert.DeserializeObject<List<SessionDtoForJsonData>>(string.Join("", sessionsJson));
            // sessions.ForEach(session => session.Speakers.ForEach(speaker => speakers.Find()));
            // sessions.Select(session => new { id = session.Id, speakers = speakers.ForEach() })
            // initialize data using the json file
            foreach (var session in sessionsDto)
            {
                var speakersList = new List<SpeakerDtoForJsonData>();
                session.Speakers.ForEach(s =>
                {
                    var existingSpeaker = speakersDto.Find(speaker => speaker.Id == s.Id);
                    speakersList.Add(existingSpeaker);
                });
                session.Speakers = speakersList;
            }

            // sanitize data, session dto has an id that can be guid or long, (currently using string to), convert to GUID(Create new Guid)
            var sessions = new List<Session>();
            foreach (var session in sessionsDto)
            {
                var sessionModel = new Session()
                {
                    Id = Guid.NewGuid(),
                    Day = session.Day,
                    Description = session.Description,
                    Favorite = session.Favorite,
                    Format = session.Format,
                    Level = session.Level,
                    Name = session.Name,
                    Room = session.Room,
                    Title = session.Title,
                    Track = session.Track,
                    StartsAt = session.StartsAt,
                    EndsAt = session.EndsAt
                };
                sessionModel.Speakers = session.Speakers.Select(s => new Speaker()
                    { Id = s.Id, Bio = s.Bio, Featured = s.Featured, Name = s.Name, }).ToList();
                sessions.Add(sessionModel);
            }

            var speakers = new List<Speaker>();
            sessions.ForEach(s => speakers.AddRange(s.Speakers));

            var distinctSpeakers = speakers.Distinct(new SpeakerComparer()).ToList();

            // get all speakers and make sure they're unique
            // var speakersCount = speakers.Distinct(new SpeakerComparer()).Count();
            foreach (var speaker in distinctSpeakers)
            {
                sessions.ForEach(se =>
                {
                    if (se.Speakers.Any(sp => sp.Id == speaker.Id))
                    {
                        speaker.Sessions.Add(se);
                    }
                });
            }
            
            sessions.ForEach(se => context.Add(se));
            distinctSpeakers.ForEach(sp => context.Add(sp));
            context.SaveChanges();
        }
    }
}