using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Api.GraphQL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Api.GraphQL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InitializeController : ControllerBase
    {
        public IActionResult Get()
        {
            // read the json file
            var  file = Path.Combine(Directory.GetCurrentDirectory(), $"Data","speakers.json");
            var s = System.IO.File.ReadAllLines(file);
            var speakers = JsonConvert.DeserializeObject<List<Speaker>>(string.Join("",s));
            
            // initialize data using the json file
            return Ok();
        }
    }
}