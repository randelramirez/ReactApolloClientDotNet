using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Api.GraphQL.Models;
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
            return Ok("Test");
        }
    }
}