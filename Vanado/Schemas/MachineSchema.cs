using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Vanado.DAL;
using Vanado.Models;

namespace Vanado.Schemas
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
