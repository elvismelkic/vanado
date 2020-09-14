using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http;
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
    [Route("files")]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost]
        public IActionResult UploadFile(IFormCollection formCollection)
        {
            var failureId = Int32.Parse(formCollection[formCollection.Keys.First().ToString()].ToString());
            var files = formCollection.Files;

            _fileService.Insert(files, failureId);

            return Ok(_fileService.GetForFailure(failureId));
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteFile(int id)
        {
            return Ok(_fileService.Delete(id));
        }
    }
}
