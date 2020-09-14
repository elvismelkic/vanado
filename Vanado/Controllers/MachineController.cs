using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Npgsql;
using WebApplication3.DAL;
using WebApplication3.Interfaces;
using WebApplication3.Models;
using WebApplication3.Services;

namespace WebApplication3.Controllers
{
    [ApiController]
    [Route("machines")]
    public class MachineController : ControllerBase
    {
        private readonly IMachineService _machineService;

        public MachineController(IMachineService machineService)
        {
            _machineService = machineService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_machineService.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(_machineService.GetById(id));
        }

        [HttpPost]
        public IActionResult Post([FromBody] MachineDTO machine)
        {
            return Ok(_machineService.Insert(machine));
        }

        [HttpPut("{id}")]
        public IActionResult Put([FromBody] MachineDTO machine, int id)
        {
            machine.Id = id;

            return Ok(_machineService.Update(machine));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return Ok(_machineService.Delete(id));
        }
    }
}
