using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vanado.Models;

namespace Vanado.Interfaces
{
    public interface IFailureRepository : IGeneralRepository<Failure>
    {
        IEnumerable<Failure> GetNotFixed();
        List<Failure> GetForMachine(int machineId);
        bool ToggleStatus(int id);
        bool DoesExist(int id);
    }
}
