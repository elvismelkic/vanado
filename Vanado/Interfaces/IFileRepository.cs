using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication3.Models;

namespace WebApplication3.Interfaces
{
    public interface IFileRepository
    {
        IEnumerable<File> GetForFailure(int failureId);
        File GetById(int id);
        bool Insert(File file);
        bool Delete(int id);
        bool IsNameTaken(string fileName, int failureId);
    }
}
