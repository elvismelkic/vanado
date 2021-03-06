﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vanado.Models;

namespace Vanado.Interfaces
{
    public interface IFailureService
    {
        IEnumerable<FailureDTO> GetAll();
        FailureFullDTO GetById(int id);
        int Insert(FailureInputDTO failureDto);
        bool Update(FailureInputDTO failureDto);
        bool Delete(int id);
        IEnumerable<FailureFullDTO> GetNotFixed();
        List<FailureDTO> GetForMachine(int machineId);
        bool ToggleStatus(int id);
        bool DoesExist(int id);
    }
}
