using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication3.Models;

namespace WebApplication3.Interfaces
{
    public interface IFailureRepository : IGeneralRepository<Failure>
    {
        IEnumerable<Failure> GetNotFixed();
        List<Failure> GetForMachine(int machineId);
        bool ToggleStatus(int id);
        bool DoesExist(int id);
    }
}
