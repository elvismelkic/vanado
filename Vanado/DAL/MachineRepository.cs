using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WebApplication3.CustomExceptions;
using WebApplication3.Interfaces;
using WebApplication3.Models;
using WebApplication3.Schemas;

namespace WebApplication3.DAL
{
    public class MachineRepository : IMachineRepository
    {
        private readonly IDbConnectionProvider _dbConnectionProvider;

        public MachineRepository(IDbConnectionProvider dbConnectionProvider)
        {
            _dbConnectionProvider = dbConnectionProvider;
        }

        private IDbConnection getNewDbConnection()
        {
            return _dbConnectionProvider.GetDbConnection();
        }

        public IEnumerable<Machine> GetAll()
        {
            using (var connection = getNewDbConnection())
            {
                var sql = $@"
                    SELECT *
                    FROM {MachineSchema.Table}
                    ORDER BY {MachineSchema.Columns.Name};
                ";

                IEnumerable<Machine> machines;

                try
                {
                    connection.Open();
                    Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

                    machines = connection.Query<Machine>(sql);
                }
                catch (PostgresException exception)
                {
                    throw new BadRequestException(exception.Message);
                }
                catch (Exception)
                {
                    throw new InternalServerErrorException();
                }

                return machines;
            }
        }

        public Machine GetById(int id)
        {
            using (var connection = getNewDbConnection())
            {
                var sql = $@"
                    SELECT *
                    FROM {MachineSchema.Table}
                    WHERE {MachineSchema.Columns.Id} = @id;
                ";

                Machine machine;

                try
                {
                    connection.Open();
                    Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

                    machine = connection.Query<Machine>(sql, new { id }).SingleOrDefault();
                }
                catch (PostgresException exception)
                {
                    throw new BadRequestException(exception.Message);
                }
                catch (Exception)
                {
                    throw new InternalServerErrorException();
                }

                if (machine == null)
                    throw new NotFoundException("Machine not found");

                return machine;
            }
        }

        public int Insert(Machine machine)
        {
            using (var connection = getNewDbConnection())
            {
                var sql = $@"
                    INSERT INTO {MachineSchema.Table} ({MachineSchema.Columns.Name})
                    VALUES (@Name)
                    RETURNING id;
                ";

                int insertedMachineId;

                try
                {
                    connection.Open();
                    Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

                    insertedMachineId = connection.QuerySingle<int>(sql, new { machine.Name });
                }
                catch (PostgresException exception)
                {
                    throw new BadRequestException(exception.Message);
                }
                catch (Exception)
                {
                    throw new InternalServerErrorException();
                }

                return insertedMachineId;
            }
        }

        public bool Update(Machine machine)
        {
            using (var connection = getNewDbConnection())
            {
                var sql = $@"
                    UPDATE {MachineSchema.Table}
                    SET {MachineSchema.Columns.Name} = @Name
                    WHERE {MachineSchema.Columns.Id} = @Id;
                ";

                int rowsAffected;

                try
                {
                    connection.Open();
                    Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

                    rowsAffected = connection.Execute(sql, new { machine.Id, machine.Name });
                }
                catch (PostgresException exception)
                {
                    throw new BadRequestException(exception.Message);
                }
                catch (Exception)
                {
                    throw new InternalServerErrorException();
                }

                if (rowsAffected == 0)
                    throw new NotFoundException("Machine not found");

                return true;
            }
        }

        public bool Delete(int id)
        {
            using (var connection = getNewDbConnection())
            {
                var sql = $@"
                    DELETE FROM {MachineSchema.Table}
                    WHERE {MachineSchema.Columns.Id} = @id;
                ";

                int rowsAffected;

                try
                {
                    connection.Open();
                    Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

                    rowsAffected = connection.Execute(sql, new { id });
                }
                catch (PostgresException exception)
                {
                    throw new BadRequestException(exception.Message);
                }
                catch (Exception)
                {
                    throw new InternalServerErrorException();
                }

                if (rowsAffected == 0)
                    throw new NotFoundException("Machine not found");

                return true;
            }
        }
    }
}
