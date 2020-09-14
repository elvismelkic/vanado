using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Vanado.Interfaces
{
    public interface IGeneralRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> GetAll();
        TEntity GetById(int Id);
        int Insert(TEntity entity);
        bool Update(TEntity entity);
        bool Delete(int id);
    }
}
