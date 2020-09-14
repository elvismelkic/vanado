using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using WebApplication3.DAL;
using WebApplication3.Models;

namespace WebApplication3.Interfaces
{
    public interface IMachineService
    {
        IEnumerable<MachineDTO> GetAll();
        MachineDTO GetById(int id);
        int Insert(MachineDTO machineDto);
        bool Update(MachineDTO machineDto);
        bool Delete(int id);
    }
}
