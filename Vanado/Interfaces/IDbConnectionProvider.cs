using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Vanado.Models;

namespace Vanado.Interfaces
{
    public interface IDbConnectionProvider
    {
        IDbConnection GetDbConnection();
    }
}
