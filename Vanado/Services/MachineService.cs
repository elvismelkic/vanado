using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using WebApplication3.DAL;
using WebApplication3.Interfaces;
using WebApplication3.Models;

namespace WebApplication3.Services
{
    public class MachineService : IMachineService
    {
        private readonly IMachineRepository _machineRepository;
        private readonly IFailureService _failureService;
        private readonly IFileService _fileService;

        public MachineService(IMachineRepository machineRepository, IFailureService failureService, IFileService fileService)
        {
            _machineRepository = machineRepository;
            _failureService = failureService;
            _fileService = fileService;
        }

        public IEnumerable<MachineDTO> GetAll()
        {
            var machines = _machineRepository.GetAll();
            var machineDtos = machines.Select(machine => new MachineDTO() { Id = machine.Id, Name = machine.Name });

            return machineDtos;
        }

        public MachineDTO GetById(int id)
        {
            var machine = _machineRepository.GetById(id);
            var failures = _failureService.GetForMachine(id);
            var machineDto = new MachineDTO() { Id = machine.Id, Name = machine.Name, Failures = failures };

            return machineDto;
        }

        public int Insert(MachineDTO machineDto)
        {
            var machine = new Machine { Id = machineDto.Id, Name = machineDto.Name };

            return _machineRepository.Insert(machine);
        }

        public bool Update(MachineDTO machineDto)
        {
            var machine = new Machine { Id = machineDto.Id, Name = machineDto.Name };

            return _machineRepository.Update(machine);
        }

        public bool Delete(int id)
        {
            var failures = _failureService.GetForMachine(id);
            var result = _machineRepository.Delete(id);

            foreach (var failure in failures)
            {
                _fileService.DeleteFolder(failure.Id);
            }

            return result;
        }
    }
}
