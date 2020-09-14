using Dapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Vanado.DAL;
using Vanado.Models;

namespace Vanado.Schemas
{
    public static class FileSchema
    {
        public static string Table { get; } = "files";

        public static class Columns
        {
            public static string Id { get; } = "id";
            public static string Name { get; } = "name";
            public static string Type { get; } = "type";
            public static string FailureId { get; } = "failure_id";
        }
    }
}