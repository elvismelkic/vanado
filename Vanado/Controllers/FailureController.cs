using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using Vanado.DAL;
using Vanado.Interfaces;
using Vanado.Models;
using Vanado.Services;

namespace Vanado.Controllers
{
    [ApiController]
    [Route("failures")]
    public class FailureController : ControllerBase
    {
        private readonly IFailureService _failureService;

        public FailureController(IFailureService failureService)
        {
            _failureService = failureService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_failureService.GetNotFixed());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(_failureService.GetById(id));
        }

        [HttpPost]
        public IActionResult Post([FromBody] FailureInputDTO failure)
        {
            return Ok(_failureService.Insert(failure));
        }

        [HttpPut("{id}")]
        public IActionResult Put([FromBody] FailureInputDTO failure, int id)
        {
            failure.Id = id;

            return Ok(_failureService.Update(failure));
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(int id)
        {
            return Ok(_failureService.ToggleStatus(id));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return Ok(_failureService.Delete(id));
        }
    }
}
