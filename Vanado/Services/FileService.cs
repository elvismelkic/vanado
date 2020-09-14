using Dapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Vanado.CustomExceptions;
using Vanado.DAL;
using Vanado.Interfaces;
using Vanado.Models;

namespace Vanado.Services
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepository;

        public FileService(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }

        private string GenerateFailureFolderPath(int failureId)
        {
            return $"./ClientApp/public/files/failure_{failureId}/";
        }

        public IEnumerable<FileDTO> GetForFailure(int failureId)
        {
            var files = _fileRepository.GetForFailure(failureId);
            var fileDtos = files.Select(file => new FileDTO()
            {
                Id = file.Id,
                Name = file.Name,
                Type = file.Type,
                FailureId = file.FailureId
            });

            return fileDtos;
        }

        public bool Insert(IFormFileCollection files, int failureId)
        {
            var folderPath = GenerateFailureFolderPath(failureId);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var hasTakenName = IsAnyNameTaken(files, failureId);

            if (hasTakenName)
                throw new ExistingFileException();
            
            return StoreAndSaveToDb(files, folderPath, failureId);
        }

        private bool IsAnyNameTaken(IFormFileCollection files, int failureId)
        {
            var hasTakenName = false;

            foreach (var file in files)
            {
                hasTakenName = _fileRepository.IsNameTaken(file.FileName, failureId);

                if (hasTakenName)
                    break;
            }

            return hasTakenName;
        }

        private bool StoreAndSaveToDb(IFormFileCollection files, string folderPath, int failureId)
        {
            foreach (var file in files)
            {
                var path = Path.Combine(folderPath + file.FileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);

                    var fileInstance = new Models.File() { Name = file.FileName, Type = file.ContentType, FailureId = failureId };

                    _fileRepository.Insert(fileInstance);
                }
            }
            return true;
        }

        public IEnumerable<FileDTO> Delete(int id)
        {
            var file = _fileRepository.GetById(id);
            var folderPath = GenerateFailureFolderPath(file.FailureId);
            var path = Path.Combine(folderPath + file.Name);

            _fileRepository.Delete(id);

            try
            {
                System.IO.File.Delete(path);
            }
            catch (Exception)
            {
                throw new InternalServerErrorException();
            }

            return GetForFailure(file.FailureId);
        }

        public bool DeleteFolder(int failureId)
        {
            var folderPath = GenerateFailureFolderPath(failureId);

            try
            {
                if (Directory.Exists(folderPath))
                    Directory.Delete(folderPath, true);
            }
            catch (Exception)
            {

                throw new InternalServerErrorException();
            }

            return true;
        }
    }
}
