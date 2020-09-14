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
using Vanado.CustomExceptions;
using Vanado.DAL;
using Vanado.Models;
using Vanado.Services;

namespace Vanado.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController : ControllerBase
    {
        [Route("error")]
        public ErrorResponse Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context?.Error; // My exception
            var code = 500; // Internal Server Error by default

            if (exception is NotFoundException) code = 404;
            else if (exception is BadRequestException) code = 400;

            Response.StatusCode = code;

            return new ErrorResponse(exception);
        }
    }
}
