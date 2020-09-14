using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Vanado.DAL;
using Vanado.Models;

namespace Vanado.Interfaces
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
