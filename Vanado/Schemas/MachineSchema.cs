using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using WebApplication3.DAL;
using WebApplication3.Models;

namespace WebApplication3.Schemas
{
    public static class MachineSchema
    {
        public static string Table { get; } = "machines";

        public static class Columns
        {
            public static string Id { get; } = "id";
            public static string Name { get; } = "name";
        }
    }
}
