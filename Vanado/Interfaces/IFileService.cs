using Dapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Vanado.DAL;
using Vanado.Models;

namespace Vanado.Interfaces
{
    public interface IFileService
    {
        IEnumerable<FileDTO> GetForFailure(int failureId);
        bool Insert(IFormFileCollection files, int failureId);
        IEnumerable<FileDTO> Delete(int id);
        bool DeleteFolder(int failureId);
    }
}
