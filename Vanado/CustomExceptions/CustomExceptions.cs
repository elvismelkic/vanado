using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Npgsql;
using WebApplication3.DAL;
using WebApplication3.Models;
using WebApplication3.Services;

namespace WebApplication3.CustomExceptions
{
    public class ErrorResponse
    {
        public string Type { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }

        public ErrorResponse(Exception ex)
        {
            Type = ex.GetType().Name;
            Message = ex.Message;
            StackTrace = ex.ToString();
        }
    }

    public class HttpStatusException : Exception
    {
        public HttpStatusCode Status { get; private set; }

        public HttpStatusException(HttpStatusCode status, string msg) : base(msg)
        {
            Status = status;
        }
    }

    public class NotFoundException : HttpStatusException
    {
        public NotFoundException(string msg) : base(HttpStatusCode.NotFound, msg) { }
    }

    public class BadRequestException : HttpStatusException
    {
        public BadRequestException(string msg) : base(HttpStatusCode.BadRequest, msg) { }
    }

    public class ExistingFileException : HttpStatusException
    {
        public ExistingFileException() : base(HttpStatusCode.BadRequest, "At least one file already exists.") { }
    }

    public class InternalServerErrorException : HttpStatusException
    {
        public InternalServerErrorException() : base(HttpStatusCode.InternalServerError, "Something went wrong") { }
    }
}
