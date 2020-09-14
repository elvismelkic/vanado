using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using WebApplication3.Controllers;
using WebApplication3.CustomExceptions;
using WebApplication3.Interfaces;
using WebApplication3.Models;
using WebApplication3.Schemas;

namespace WebApplication3.DAL
{
    public class FailureRepository : IFailureRepository
    {
        private readonly IDbConnectionProvider _dbConnectionProvider;

        public FailureRepository(IDbConnectionProvider dbConnectionProvider)
        {
            _dbConnectionProvider = dbConnectionProvider;
        }

        private IDbConnection getNewDbConnection()
        {
            return _dbConnectionProvider.GetDbConnection();
        }

        public IEnumerable<Failure> GetAll()
        {
            using (var connection = getNewDbConnection())
            {
                var sql = $@"
                    SELECT * FROM {FailureSchema.Table} f
                    JOIN {MachineSchema.Table} m ON f.{FailureSchema.Columns.MachineId} = m.{MachineSchema.Columns.Id}
                    ORDER BY { FailureSchema.Columns.Priority} DESC, { FailureSchema.Columns.InsertedAt} DESC;
                ";

                IEnumerable<Failure> failures;

                try
                {
                    connection.Open();
                    Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

                    failures = connection.Query<Failure, Machine, Failure>(sql, (failure, machine) => { failure.Machine = machine; return failure; });
                }
                catch (PostgresException exception)
                {
                    throw new BadRequestException(exception.Message);
                }
                catch (Exception)
                {
                    throw new InternalServerErrorException();
                }

                return failures;
            }
        }

        public Failure GetById(int id)
        {
            using (var connection = getNewDbConnection())
            {
                var sql = $@"
                    SELECT * FROM {FailureSchema.Table} f
                    JOIN {MachineSchema.Table} m ON f.{FailureSchema.Columns.MachineId} = m.{MachineSchema.Columns.Id}
                    WHERE f.{FailureSchema.Columns.Id} = @id;
                ";

                Failure failure;

                try
                {
                    connection.Open();
                    Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

                    failure = connection.Query<Failure, Machine, Failure>(sql, (failure, machine) => { failure.Machine = machine; return failure; }, new { id }).SingleOrDefault();
                }
                catch (PostgresException exception)
                {
                    throw new BadRequestException(exception.Message);
                }
                catch (Exception)
                {
                    throw new InternalServerErrorException();
                }

                if (failure == null)
                    throw new NotFoundException("Failure not found");

                return failure;
            }
        }

        public int Insert(Failure failure)
        {
            using (var connection = getNewDbConnection())
            {
                var sql = $@"
                    INSERT INTO {FailureSchema.Table} (
                        {FailureSchema.Columns.Name},
                        {FailureSchema.Columns.Description},
                        {FailureSchema.Columns.IsFixed},
                        {FailureSchema.Columns.Priority},
                        {FailureSchema.Columns.MachineId},
                        {FailureSchema.Columns.InsertedAt},
                        {FailureSchema.Columns.UpdatedAt}
                    )
                    VALUES (@Name, @Description, @IsFixed, @Priority::priority, @MachineId, @InsertedAt, @UpdatedAt)
                    RETURNING id;
                ";

                int insertedFailureId;

                try
                {
                    connection.Open();
                    Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

                    insertedFailureId = connection.QuerySingle<int>(sql, new { failure.Name, failure.Description, failure.IsFixed, failure.Priority, failure.MachineId, InsertedAt = DateTime.Now, UpdatedAt = DateTime.Now });
                }
                catch (PostgresException exception)
                {
                    throw new BadRequestException(exception.Message);
                }
                catch (Exception)
                {
                    throw new InternalServerErrorException();
                }

                return insertedFailureId;
            }
        }

        public bool Update(Failure failure)
        {
            using (var connection = getNewDbConnection())
            {
                var sql = $@"
                    UPDATE {FailureSchema.Table}
                    SET {FailureSchema.Columns.Name} = @Name,
                        {FailureSchema.Columns.Description} = @Description,
                        {FailureSchema.Columns.IsFixed} = @IsFixed,
                        {FailureSchema.Columns.Priority} = @Priority::priority,
                        {FailureSchema.Columns.MachineId} = @MachineId,
                        {FailureSchema.Columns.UpdatedAt} = @UpdatedAt
                    WHERE {FailureSchema.Columns.Id} = @Id;
                ";

                int rowsAffected;

                try
                {
                    connection.Open();
                    Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

                    rowsAffected = connection.Execute(sql, new { failure.Id, failure.Name, failure.Description, failure.IsFixed, failure.Priority, failure.MachineId, UpdatedAt = DateTime.Now });
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
                    throw new NotFoundException("Failure not found");

                return true;
            }
        }

        public bool Delete(int id)
        {
            using (var connection = getNewDbConnection())
            {
                var sql = $@"
                    DELETE FROM {FailureSchema.Table}
                    WHERE {FailureSchema.Columns.Id} = @id;
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
                    throw new NotFoundException("Failure not found");

                return true;
            }
        }

        public IEnumerable<Failure> GetNotFixed()
        {
            return GetAll().Where(failure => failure.IsFixed == false);
        }

        public List<Failure> GetForMachine(int machineId)
        {
            return GetAll().Where(failure => failure.MachineId == machineId).OrderBy(failure => failure.Id).ToList();
        }

        public bool ToggleStatus(int id)
        {
            using (var connection = getNewDbConnection())
            {
                var sql = $@"
                    UPDATE {FailureSchema.Table}
                    SET {FailureSchema.Columns.IsFixed} = NOT {FailureSchema.Columns.IsFixed},
                        {FailureSchema.Columns.UpdatedAt} = @UpdatedAt
                    WHERE {FailureSchema.Columns.Id} = @Id
                    RETURNING {FailureSchema.Columns.IsFixed};
                ";

                bool isFixed;

                try
                {
                    connection.Open();
                    Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

                    isFixed = connection.QuerySingle<bool>(sql, new { UpdatedAt = DateTime.Now, Id = id });
                }
                catch (PostgresException exception)
                {
                    throw new BadRequestException(exception.Message);
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("contains no elements"))
                        throw new NotFoundException("Failure not found");

                    throw new InternalServerErrorException();
                }

                return isFixed;
            }
        }

        public bool DoesExist(int id)
        {
            using (var connection = getNewDbConnection())
            {
                var sql = $@"
                    SELECT {FailureSchema.Columns.Id} FROM {FailureSchema.Table}
                    WHERE {FailureSchema.Columns.Id} = @id;
                ";

                int? fetchedId;

                try
                {
                    connection.Open();
                    Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

                    fetchedId = connection.QuerySingleOrDefault<int>(sql, new { Id = id });
                }
                catch (PostgresException exception)
                {
                    throw new BadRequestException(exception.Message);
                }
                catch (Exception)
                {
                    throw new InternalServerErrorException();
                }

                if (fetchedId == null)
                    throw new NotFoundException("Failure not found");

                return false;
            }
        }
    }
}
