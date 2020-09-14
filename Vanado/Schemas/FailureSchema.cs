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
    public static class FailureSchema
    {
        public static string Table { get; } = "failures";

        public static class Columns
        {
            public static string Id { get; } = "id";
            public static string Name { get; } = "name";
            public static string Description { get; } = "description";
            public static string IsFixed { get; } = "is_fixed";
            public static string MachineId { get; } = "machine_id";
            public static string Priority { get; } = "priority";
            public static string InsertedAt { get; } = "inserted_at";
            public static string UpdatedAt { get; } = "updated_at";
        }
    }
}