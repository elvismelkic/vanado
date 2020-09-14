using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing.Constraints;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Vanado.Controllers;
using Vanado.CustomExceptions;
using Vanado.Interfaces;
using Vanado.Models;
using Vanado.Schemas;

namespace Vanado.DAL
{
    public class FileRepository : IFileRepository
    {
        private readonly IDbConnectionProvider _dbConnectionProvider;

        public FileRepository(IDbConnectionProvider dbConnectionProvider)
        {
            _dbConnectionProvider = dbConnectionProvider;
        }

        private IDbConnection getNewDbConnection()
        {
            return _dbConnectionProvider.GetDbConnection();
        }

        public IEnumerable<Models.File> GetForFailure(int failureId)
        {
            using (var connection = getNewDbConnection())
            {
                var sql = $@"
                    SELECT *
                    FROM {FileSchema.Table}
                    WHERE {FileSchema.Columns.FailureId} = @failureId;
                ";

                IEnumerable<File> failureFiles;

                try
                {
                    connection.Open();
                    Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

                    failureFiles = connection.Query<Models.File>(sql, new { failureId });
                }
                catch (PostgresException exception)
                {
                    throw new BadRequestException(exception.Message);
                }
                catch (Exception)
                {
                    throw new InternalServerErrorException();
                }

                return failureFiles;
            }
        }

        public File GetById(int id)
        {
            using (var connection = getNewDbConnection())
            {
                var sql = $@"
                    SELECT *
                    FROM {FileSchema.Table}
                    WHERE {FileSchema.Columns.Id} = @id;
                ";

                File file;

                try
                {
                    connection.Open();
                    Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

                    file = connection.QuerySingleOrDefault<Models.File>(sql, new { id });
                }
                catch (PostgresException exception)
                {
                    throw new BadRequestException(exception.Message);
                }
                catch (Exception)
                {
                    throw new InternalServerErrorException();
                }

                if (file == null)
                    throw new NotFoundException("File not found");

                return file;
            }
        }

        public bool Insert(File file)
        {
            using (var connection = getNewDbConnection())
            {
                var sql = $@"
                    INSERT INTO {FileSchema.Table} (
                        {FileSchema.Columns.Name},
                        {FileSchema.Columns.Type},
                        {FileSchema.Columns.FailureId}
                    )
                    VALUES (@Name, @Type, @FailureId);
                ";

                int rowsAffected;

                try
                {
                    connection.Open();
                    Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

                    rowsAffected = connection.Execute(sql, new { file.Name, file.Type, file.FailureId });
                }
                catch (PostgresException exception)
                {
                    throw new BadRequestException(exception.Message);
                }
                catch (Exception)
                {
                    throw new InternalServerErrorException();
                }

                if (rowsAffected > 0)
                    return true;

                return false;
            }
        }

        public bool Delete(int id)
        {
            using (var connection = getNewDbConnection())
            {
                var sql = $@"
                    DELETE FROM {FileSchema.Table}
                    WHERE {FileSchema.Columns.Id} = @id;
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
                    throw new NotFoundException("File not found");

                return true;
            }
        }

        public bool IsNameTaken(string fileName, int failureId)
        {
            using (var connection = getNewDbConnection())
            {
                var sql = $@"
                    SELECT {FileSchema.Columns.Name} FROM {FileSchema.Table}
                    WHERE {FileSchema.Columns.Name} = @fileName
                        AND {FileSchema.Columns.FailureId} = @failureId;
                ";

                string fetchedFileName;

                try
                {

                    connection.Open();
                    Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

                    fetchedFileName = connection.QuerySingleOrDefault<string>(sql, new { fileName, failureId });
                }
                catch (PostgresException exception)
                {
                    throw new BadRequestException(exception.Message);
                }
                catch (Exception)
                {
                    throw new InternalServerErrorException();
                }

                if (fetchedFileName != null)
                    return true;

                return false;
            }
        }
    }
}
