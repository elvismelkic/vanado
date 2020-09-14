using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using WebApplication3.DAL;
using WebApplication3.Interfaces;
using WebApplication3.Models;

namespace WebApplication3.Services
{
    public class FailureService : IFailureService
    {
        private readonly IFailureRepository _failureRepository;
        private readonly IFileService _fileService;

        public FailureService(IFailureRepository failureRepository, IFileService fileService)
        {
            _failureRepository = failureRepository;
            _fileService = fileService;
        }

        public IEnumerable<FailureDTO> GetAll()
        {
            var failures = _failureRepository.GetAll();
            var failureDtos = failures.Select(failure => new FailureDTO() {
                Id = failure.Id,
                Name = failure.Name,
                Description = failure.Description,
                IsFixed = failure.IsFixed,
                Priority = failure.Priority
            });

            return failureDtos;
        }

        public FailureFullDTO GetById(int id)
        {
            var failure = _failureRepository.GetById(id);
            var failureFiles = _fileService.GetForFailure(id).ToList();
            var failureDto = new FailureFullDTO()
            {
                Id = failure.Id,
                Name = failure.Name,
                Description = failure.Description,
                IsFixed = failure.IsFixed,
                Priority = failure.Priority,
                MachineId = failure.Machine.Id,
                MachineName = failure.Machine.Name,
                Files = failureFiles
            };

            return failureDto;
        }

        public int Insert(FailureInputDTO failureDto)
        {
            var failure = new Failure { 
                Id = failureDto.Id,
                Name = failureDto.Name,
                Description = failureDto.Description,
                IsFixed = failureDto.IsFixed,
                Priority = failureDto.Priority,
                MachineId = failureDto.MachineId
            };

            return _failureRepository.Insert(failure);
        }

        public bool Update(FailureInputDTO failureDto)
        {
            var failure = new Failure
            {
                Id = failureDto.Id,
                Name = failureDto.Name,
                Description = failureDto.Description,
                IsFixed = failureDto.IsFixed,
                Priority = failureDto.Priority,
                MachineId = failureDto.MachineId
            };

            return _failureRepository.Update(failure);
        }

        public bool Delete(int id)
        {
            _failureRepository.Delete(id);
            return _fileService.DeleteFolder(id);
        }

        public IEnumerable<FailureFullDTO> GetNotFixed()
        {
            var failures = _failureRepository.GetNotFixed();
            var failureDtos = failures.Select(failure => new FailureFullDTO()
            {
                Id = failure.Id,
                Name = failure.Name,
                Description = failure.Description,
                IsFixed = failure.IsFixed,
                Priority = failure.Priority,
                MachineName = failure.Machine.Name
            });

            return failureDtos;
        }

        public List<FailureDTO> GetForMachine(int machineId)
        {
            var failures = _failureRepository.GetForMachine(machineId);
            var failureDtos = failures.Select(failure => new FailureDTO()
            {
                Id = failure.Id,
                Name = failure.Name,
                Description = failure.Description,
                IsFixed = failure.IsFixed,
                Priority = failure.Priority
            });

            return failureDtos.ToList();
        }

        public bool ToggleStatus(int id)
        {
            return _failureRepository.ToggleStatus(id);
        }

        public bool DoesExist(int id)
        {
            return _failureRepository.DoesExist(id);
        }
    }
}
