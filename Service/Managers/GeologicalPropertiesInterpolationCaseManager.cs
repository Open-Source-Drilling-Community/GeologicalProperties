using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using OSDC.DotnetLibraries.General.DataManagement;
using Microsoft.Data.Sqlite;
using System.Text.Json;

namespace GeologicalProperties.Service.Managers
{
    /// <summary>
    /// A manager for GeologicalPropertiesInterpolationCase. The manager implements the singleton pattern as defined by 
    /// Gamma, Erich, et al. "Design patterns: Abstraction and reuse of object-oriented design." 
    /// European Conference on Object-Oriented Programming. Springer, Berlin, Heidelberg, 1993.
    /// </summary>
    public class GeologicalPropertiesInterpolationCaseManager
    {
        private static GeologicalPropertiesInterpolationCaseManager? _instance = null;
        private readonly ILogger<GeologicalPropertiesInterpolationCaseManager> _logger;
        private readonly SqlConnectionManager _connectionManager;

        private GeologicalPropertiesInterpolationCaseManager(ILogger<GeologicalPropertiesInterpolationCaseManager> logger, SqlConnectionManager connectionManager)
        {
            _logger = logger;
            _connectionManager = connectionManager;
        }

        public static GeologicalPropertiesInterpolationCaseManager GetInstance(ILogger<GeologicalPropertiesInterpolationCaseManager> logger, SqlConnectionManager connectionManager)
        {
            _instance ??= new GeologicalPropertiesInterpolationCaseManager(logger, connectionManager);
            return _instance;
        }

        public int Count
        {
            get
            {
                int count = 0;
                var connection = _connectionManager.GetConnection();
                if (connection != null)
                {
                    var command = connection.CreateCommand();
                    command.CommandText = "SELECT COUNT(*) FROM GeologicalPropertiesInterpolationCaseTable";
                    try
                    {
                        using SqliteDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            count = (int)reader.GetInt64(0);
                        }
                    }
                    catch (SqliteException ex)
                    {
                        _logger.LogError(ex, "Impossible to count records in the GeologicalPropertiesInterpolationCaseTable");
                    }
                }
                else
                {
                    _logger.LogWarning("Impossible to access the SQLite database");
                }
                return count;
            }
        }

        public bool Clear()
        {
            var connection = _connectionManager.GetConnection();
            if (connection != null)
            {
                bool success = false;
                using var transaction = connection.BeginTransaction();
                try
                {
                    //empty GeologicalPropertiesTable
                    var command = connection.CreateCommand();
                    command.CommandText = "DELETE FROM GeologicalPropertiesInterpolationCaseTable";
                    command.ExecuteNonQuery();

                    transaction.Commit();
                    success = true;
                }
                catch (SqliteException ex)
                {
                    transaction.Rollback();
                    _logger.LogError(ex, "Impossible to clear the GeologicalPropertiesInterpolationCaseTable");
                }
                return success;
            }
            else
            {
                _logger.LogWarning("Impossible to access the SQLite database");
                return false;
            }
        }

        public bool Contains(Guid guid)
        {
            int count = 0;
            var connection = _connectionManager.GetConnection();
            if (connection != null)
            {
                var command = connection.CreateCommand();
                command.CommandText = $"SELECT COUNT(*) FROM GeologicalPropertiesInterpolationCaseTable WHERE ID = '{guid}'";
                try
                {
                    using SqliteDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        count = (int)reader.GetInt64(0);
                    }
                }
                catch (SqliteException ex)
                {
                    _logger.LogError(ex, "Impossible to count rows from GeologicalPropertiesInterpolationCaseTable");
                }
            }
            else
            {
                _logger.LogWarning("Impossible to access the SQLite database");
            }
            return count >= 1;
        }

        /// <summary>
        /// Returns the list of Guid of all GeologicalPropertiesInterpolationCase present in the microservice database 
        /// </summary>
        /// <returns>the list of Guid of all GeologicalPropertiesInterpolationCase present in the microservice database</returns>
        public List<Guid>? GetAllGeologicalPropertiesInterpolationCaseId()
        {
            List<Guid> ids = [];
            var connection = _connectionManager.GetConnection();
            if (connection != null)
            {
                var command = connection.CreateCommand();
                command.CommandText = "SELECT ID FROM GeologicalPropertiesInterpolationCaseTable";
                try
                {
                    using var reader = command.ExecuteReader();
                    while (reader.Read() && !reader.IsDBNull(0))
                    {
                        Guid id = reader.GetGuid(0);
                        ids.Add(id);
                    }
                    _logger.LogInformation("Returning the list of ID of existing records from GeologicalPropertiesInterpolationCaseTable");
                    return ids;
                }
                catch (SqliteException ex)
                {
                    _logger.LogError(ex, "Impossible to get IDs from GeologicalPropertiesInterpolationCaseTable");
                }
            }
            else
            {
                _logger.LogWarning("Impossible to access the SQLite database");
            }
            return null;
        }

        /// <summary>
        /// Returns the list of MetaInfo of all GeologicalPropertiesInterpolationCase present in the microservice database 
        /// </summary>
        /// <returns>the list of MetaInfo of all GeologicalPropertiesInterpolationCase present in the microservice database</returns>
        public List<MetaInfo?>? GetAllGeologicalPropertiesInterpolationCaseMetaInfo()
        {
            List<MetaInfo?> metaInfos = new();
            var connection = _connectionManager.GetConnection();
            if (connection != null)
            {
                var command = connection.CreateCommand();
                command.CommandText = "SELECT MetaInfo FROM GeologicalPropertiesInterpolationCaseTable";
                try
                {
                    using var reader = command.ExecuteReader();
                    while (reader.Read() && !reader.IsDBNull(0))
                    {
                        string mInfo = reader.GetString(0);
                        MetaInfo? metaInfo = JsonSerializer.Deserialize<MetaInfo>(mInfo, JsonSettings.Options);
                        metaInfos.Add(metaInfo);
                    }
                    _logger.LogInformation("Returning the list of MetaInfo of existing records from GeologicalPropertiesInterpolationCaseTable");
                    return metaInfos;
                }
                catch (SqliteException ex)
                {
                    _logger.LogError(ex, "Impossible to get IDs from GeologicalPropertiesInterpolationCaseTable");
                }
            }
            else
            {
                _logger.LogWarning("Impossible to access the SQLite database");
            }
            return null;
        }

        /// <summary>
        /// Returns the GeologicalPropertiesInterpolationCase identified by its Guid from the microservice database 
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>the GeologicalPropertiesInterpolationCase identified by its Guid from the microservice database</returns>
        public Model.GeologicalPropertiesInterpolationCase? GetGeologicalPropertiesInterpolationCaseById(Guid guid)
        {
            if (!guid.Equals(Guid.Empty))
            {
                var connection = _connectionManager.GetConnection();
                if (connection != null)
                {
                    Model.GeologicalPropertiesInterpolationCase? geologicalPropertiesInterpolationCase;
                    var command = connection.CreateCommand();
                    command.CommandText = $"SELECT Data FROM GeologicalPropertiesInterpolationCaseTable WHERE ID = '{guid}'";
                    try
                    {
                        using var reader = command.ExecuteReader();
                        if (reader.Read() && !reader.IsDBNull(0))
                        {
                            string data = reader.GetString(0);
                            geologicalPropertiesInterpolationCase = JsonSerializer.Deserialize<Model.GeologicalPropertiesInterpolationCase>(data, JsonSettings.Options);
                            if (geologicalPropertiesInterpolationCase != null && geologicalPropertiesInterpolationCase.MetaInfo != null && !geologicalPropertiesInterpolationCase.MetaInfo.ID.Equals(guid))
                                throw new SqliteException("SQLite database corrupted: returned GeologicalPropertiesInterpolationCase is null or has been jsonified with the wrong ID.", 1);
                        }
                        else
                        {
                            _logger.LogInformation("No GeologicalPropertiesInterpolationCase of given ID in the database");
                            return null;
                        }
                    }
                    catch (SqliteException ex)
                    {
                        _logger.LogError(ex, "Impossible to get the GeologicalPropertiesInterpolationCase with the given ID from GeologicalPropertiesInterpolationCaseTable");
                        return null;
                    }
                    _logger.LogInformation("Returning the GeologicalPropertiesInterpolationCase of given ID from GeologicalPropertiesInterpolationCaseTable");
                    return geologicalPropertiesInterpolationCase;
                }
                else
                {
                    _logger.LogWarning("Impossible to access the SQLite database");
                }
            }
            else
            {
                _logger.LogWarning("The given GeologicalPropertiesInterpolationCase ID is null or empty");
            }
            return null;
        }

        /// <summary>
        /// Returns the list of all GeologicalPropertiesInterpolationCase present in the microservice database 
        /// </summary>
        /// <returns>the list of all GeologicalPropertiesInterpolationCase present in the microservice database</returns>
        public List<Model.GeologicalPropertiesInterpolationCase?>? GetAllGeologicalPropertiesInterpolationCase()
        {
            List<Model.GeologicalPropertiesInterpolationCase?> vals = [];
            var connection = _connectionManager.GetConnection();
            if (connection != null)
            {
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Data FROM GeologicalPropertiesInterpolationCaseTable";
                try
                {
                    using var reader = command.ExecuteReader();
                    while (reader.Read() && !reader.IsDBNull(0))
                    {
                        string data = reader.GetString(0);
                        Model.GeologicalPropertiesInterpolationCase? geologicalPropertiesInterpolationCase = JsonSerializer.Deserialize<Model.GeologicalPropertiesInterpolationCase>(data, JsonSettings.Options);
                        vals.Add(geologicalPropertiesInterpolationCase);
                    }
                    _logger.LogInformation("Returning the list of existing GeologicalPropertiesInterpolationCase from GeologicalPropertiesInterpolationCaseTable");
                    return vals;
                }
                catch (SqliteException ex)
                {
                    _logger.LogError(ex, "Impossible to get GeologicalPropertiesInterpolationCase from GeologicalPropertiesInterpolationCaseTable");
                }
            }
            else
            {
                _logger.LogWarning("Impossible to access the SQLite database");
            }
            return null;
        }

        /// <summary>
        /// Returns the list of all GeologicalPropertiesInterpolationCaseLight present in the microservice database 
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>the list of GeologicalPropertiesInterpolationCaseLight present in the microservice database</returns>
        public List<Model.GeologicalPropertiesInterpolationCaseLight>? GetAllGeologicalPropertiesInterpolationCaseLight()
        {
            List<Model.GeologicalPropertiesInterpolationCaseLight>? geologicalPropertiesLightList = [];
            var connection = _connectionManager.GetConnection();
            if (connection != null)
            {
                var command = connection.CreateCommand();
                command.CommandText = "SELECT MetaInfo, Name, Description, CreationDate, LastModificationDate, GeologicalPropertiesID FROM GeologicalPropertiesInterpolationCaseTable";
                try
                {
                    using var reader = command.ExecuteReader();
                    while (reader.Read() && !reader.IsDBNull(0))
                    {
                        string metaInfoStr = reader.GetString(0);
                        MetaInfo? metaInfo = JsonSerializer.Deserialize<MetaInfo>(metaInfoStr, JsonSettings.Options);
                        string name = reader.GetString(1);
                        string descr = reader.GetString(2);
                        // make sure DateTimeOffset are properly instantiated when stored values are null (and parsed as empty string)
                        DateTimeOffset? creationDate = null;
                        if (DateTimeOffset.TryParse(reader.GetString(3), out DateTimeOffset cDate))
                            creationDate = cDate;
                        DateTimeOffset? lastModificationDate = null;
                        if (DateTimeOffset.TryParse(reader.GetString(4), out DateTimeOffset lDate))
                            lastModificationDate = lDate;
                        Guid? geologicalPropertiesID = null;
                        if (Guid.TryParse(reader.GetString(5), out Guid ID))
                        {
                            geologicalPropertiesID = ID;
                        }
                        geologicalPropertiesLightList.Add(new Model.GeologicalPropertiesInterpolationCaseLight(
                                metaInfo,
                                string.IsNullOrEmpty(name) ? null : name,
                                string.IsNullOrEmpty(descr) ? null : descr,
                                creationDate,
                                lastModificationDate,
                                geologicalPropertiesID
                                ));
                    }
                    _logger.LogInformation("Returning the list of existing GeologicalPropertiesInterpolationCaseLight from GeologicalPropertiesInterpolationCaseTable");
                    return geologicalPropertiesLightList;
                }
                catch (SqliteException ex)
                {
                    _logger.LogError(ex, "Impossible to get light datas from GeologicalPropertiesInterpolationCaseTable");
                }
            }
            else
            {
                _logger.LogWarning("Impossible to access the SQLite database");
            }
            return null;
        }

        /// <summary>
        /// Performs calculation on the given GeologicalPropertiesInterpolationCase and adds it to the microservice database
        /// </summary>
        /// <param name="geologicalPropertiesInterpolationCase"></param>
        /// <returns>true if the given GeologicalPropertiesInterpolationCase has been added successfully to the microservice database</returns>
        public bool AddGeologicalPropertiesInterpolationCase(Model.GeologicalPropertiesInterpolationCase? geologicalPropertiesInterpolationCase)
        {
            if (geologicalPropertiesInterpolationCase != null && geologicalPropertiesInterpolationCase.MetaInfo != null && geologicalPropertiesInterpolationCase.MetaInfo.ID != Guid.Empty)
            {
                if (geologicalPropertiesInterpolationCase.GeologicalPropertiesID != null && geologicalPropertiesInterpolationCase.GeologicalPropertiesID != Guid.Empty && GeologicalPropertiesManager.Instance != null)
                {
                    Model.GeologicalProperties? geologicalProperties = GeologicalPropertiesManager.Instance.GetGeologicalPropertiesById(geologicalPropertiesInterpolationCase.GeologicalPropertiesID.Value);
                    if (geologicalProperties != null)
                    {
                        if (!geologicalPropertiesInterpolationCase.Calculate(geologicalProperties))
                        {
                            _logger.LogWarning("Impossible to calculate outputs for the given GeologicalPropertiesInterpolationCase");
                            return false;
                        }
                    }else
                    {
                        _logger.LogWarning("Could not retrieve the GeologicalProperties associated with this GeologicalPropertiesInterpolationCase");
                        return false;
                    }
                }
                //if successful, check if another parent data with the same ID was calculated/added during the calculation time
                Model.GeologicalPropertiesInterpolationCase? newGeologicalPropertiesInterpolationCase = GetGeologicalPropertiesInterpolationCaseById(geologicalPropertiesInterpolationCase.MetaInfo.ID);
                if (newGeologicalPropertiesInterpolationCase == null)
                {
                    //update GeologicalPropertiesTable
                    var connection = _connectionManager.GetConnection();
                    if (connection != null)
                    {
                        using SqliteTransaction transaction = connection.BeginTransaction();
                        bool success = true;
                        try
                        {
                            //add the GeologicalProperties to the GeologicalPropertiesTable
                            string metaInfo = JsonSerializer.Serialize(geologicalPropertiesInterpolationCase.MetaInfo, JsonSettings.Options);
                            string? cDate = null;
                            if (geologicalPropertiesInterpolationCase.CreationDate != null)
                                cDate = ((DateTimeOffset)geologicalPropertiesInterpolationCase.CreationDate).ToString(SqlConnectionManager.DATE_TIME_FORMAT);
                            string? lDate = null;
                            if (geologicalPropertiesInterpolationCase.LastModificationDate != null)
                                lDate = ((DateTimeOffset)geologicalPropertiesInterpolationCase.LastModificationDate).ToString(SqlConnectionManager.DATE_TIME_FORMAT);
                            Guid? geologicalPropertiesID = geologicalPropertiesInterpolationCase.GeologicalPropertiesID;
                            string data = JsonSerializer.Serialize(geologicalPropertiesInterpolationCase, JsonSettings.Options);
                            var command = connection.CreateCommand();
                            command.CommandText = "INSERT INTO GeologicalPropertiesInterpolationCaseTable (" +
                                "ID, " +
                                "MetaInfo, " +
                                "Name, " +
                                "Description, " +
                                "CreationDate, " +
                                "LastModificationDate, " +
                                "GeologicalPropertiesID, " +
                                "Data" +
                                ") VALUES (" +
                                $"'{geologicalPropertiesInterpolationCase.MetaInfo.ID}', " +
                                $"'{metaInfo}', " +
                                $"'{geologicalPropertiesInterpolationCase.Name}', " +
                                $"'{geologicalPropertiesInterpolationCase.Description}', " +
                                $"'{cDate}', " +
                                $"'{lDate}', " +
                                $"'{geologicalPropertiesID}', " +
                                $"'{data}'" +
                                ")";
                            int count = command.ExecuteNonQuery();
                            if (count != 1)
                            {
                                _logger.LogWarning("Impossible to insert the given GeologicalPropertiesInterpolationCase into the GeologicalPropertiesInterpolationCaseTable");
                                success = false;
                            }
                        }
                        catch (SqliteException ex)
                        {
                            _logger.LogError(ex, "Impossible to add the given GeologicalPropertiesInterpolationCase into GeologicalPropertiesInterpolationCaseTable");
                            success = false;
                        }
                        //finalizing SQL transaction
                        if (success)
                        {
                            transaction.Commit();
                            _logger.LogInformation("Added the given GeologicalPropertiesInterpolationCase of given ID into the GeologicalPropertiesInterpolationCaseTable successfully");
                        }
                        else
                        {
                            transaction.Rollback();
                        }
                        return success;
                    }
                    else
                    {
                        _logger.LogWarning("Impossible to access the SQLite database");
                    }
                }
                else
                {
                    _logger.LogWarning("Impossible to post GeologicalPropertiesInterpolationCase. ID already found in database.");
                    return false;
                }

            }
            else
            {
                _logger.LogWarning("The GeologicalPropertiesInterpolationCase ID or the ID of its input are null or empty");
            }
            return false;
        }

        /// <summary>
        /// Performs calculation on the given GeologicalPropertiesInterpolationCase and updates it in the microservice database
        /// </summary>
        /// <param name="geologicalPropertiesInterpolationCase"></param>
        /// <returns>true if the given GeologicalProperties has been updated successfully</returns>
        public bool UpdateGeologicalPropertiesInterpolationCaseById(Guid guid, Model.GeologicalPropertiesInterpolationCase? geologicalPropertiesInterpolationCase)
        {
            bool success = true;
            if (guid != Guid.Empty && geologicalPropertiesInterpolationCase != null && geologicalPropertiesInterpolationCase.MetaInfo != null && geologicalPropertiesInterpolationCase.MetaInfo.ID == guid)
            {
                if (geologicalPropertiesInterpolationCase.GeologicalPropertiesID != null && geologicalPropertiesInterpolationCase.GeologicalPropertiesID != Guid.Empty && GeologicalPropertiesManager.Instance != null)
                {
                    Model.GeologicalProperties? geologicalProperties = GeologicalPropertiesManager.Instance.GetGeologicalPropertiesById(geologicalPropertiesInterpolationCase.GeologicalPropertiesID.Value);
                    if (geologicalProperties != null)
                    {
                        if (!geologicalPropertiesInterpolationCase.Calculate(geologicalProperties))
                        {
                            _logger.LogWarning("Impossible to calculate outputs for the given GeologicalPropertiesInterpolationCase");
                            return false;
                        }
                    }
                    else
                    {
                        _logger.LogWarning("Could not retrieve the GeologicalProperties associated with this GeologicalPropertiesInterpolationCase");
                        return false;
                    }
                }
                //update GeologicalPropertiesTable
                var connection = _connectionManager.GetConnection();
                if (connection != null)
                {
                    using SqliteTransaction transaction = connection.BeginTransaction();
                    //update fields in GeologicalPropertiesTable
                    try
                    {
                        string metaInfo = JsonSerializer.Serialize(geologicalPropertiesInterpolationCase.MetaInfo, JsonSettings.Options);
                        string? cDate = null;
                        if (geologicalPropertiesInterpolationCase.CreationDate != null)
                            cDate = ((DateTimeOffset)geologicalPropertiesInterpolationCase.CreationDate).ToString(SqlConnectionManager.DATE_TIME_FORMAT);
                        geologicalPropertiesInterpolationCase.LastModificationDate = DateTimeOffset.UtcNow;
                        string? lDate = ((DateTimeOffset)geologicalPropertiesInterpolationCase.LastModificationDate).ToString(SqlConnectionManager.DATE_TIME_FORMAT);
                        string data = JsonSerializer.Serialize(geologicalPropertiesInterpolationCase, JsonSettings.Options);
                        var command = connection.CreateCommand();
                        command.CommandText = $"UPDATE GeologicalPropertiesInterpolationCaseTable SET " +
                            $"MetaInfo = '{metaInfo}', " +
                            $"Name = '{geologicalPropertiesInterpolationCase.Name}', " +
                            $"Description = '{geologicalPropertiesInterpolationCase.Description}', " +
                            $"CreationDate = '{cDate}', " +
                            $"LastModificationDate = '{lDate}', " +
                            $"GeologicalPropertiesID = '{geologicalPropertiesInterpolationCase.GeologicalPropertiesID}', "+
                            $"Data = '{data}' " +
                            $"WHERE ID = '{guid}'";
                        int count = command.ExecuteNonQuery();
                        if (count != 1)
                        {
                            _logger.LogWarning("Impossible to update the GeologicalPropertiesInterpolationCase");
                            success = false;
                        }
                    }
                    catch (SqliteException ex)
                    {
                        _logger.LogError(ex, "Impossible to update the GeologicalPropertiesInterpolationCase");
                        success = false;
                    }

                    // Finalizing
                    if (success)
                    {
                        transaction.Commit();
                        _logger.LogInformation("Updated the given GeologicalPropertiesInterpolationCase successfully");
                        return true;
                    }
                    else
                    {
                        transaction.Rollback();
                    }
                }
                else
                {
                    _logger.LogWarning("Impossible to access the SQLite database");
                }
            }
            else
            {
                _logger.LogWarning("The GeologicalPropertiesInterpolationCase ID or the ID of some of its attributes are null or empty");
            }
            return false;
        }

        /// <summary>
        /// Deletes the GeologicalPropertiesInterpolationCase of given ID from the microservice database
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>true if the GeologicalPropertiesInterpolationCase was deleted from the microservice database</returns>
        public bool DeleteGeologicalPropertiesInterpolationCaseById(Guid guid)
        {
            if (!guid.Equals(Guid.Empty))
            {
                var connection = _connectionManager.GetConnection();
                if (connection != null)
                {
                    using var transaction = connection.BeginTransaction();
                    bool success = true;
                    //delete GeologicalPropertiesInterpolationCase from GeologicalPropertiesInterpolationCaseTable
                    try
                    {
                        var command = connection.CreateCommand();
                        command.CommandText = $"DELETE FROM GeologicalPropertiesInterpolationCaseTable WHERE ID = '{guid}'";
                        int count = command.ExecuteNonQuery();
                        if (count < 0)
                        {
                            _logger.LogWarning("Impossible to delete the GeologicalPropertiesInterpolationCase of given ID from the GeologicalPropertiesInterpolationCaseTable");
                            success = false;
                        }
                    }
                    catch (SqliteException ex)
                    {
                        _logger.LogError(ex, "Impossible to delete the GeologicalPropertiesInterpolationCase of given ID from GeologicalPropertiesInterpolationCaseTable");
                        success = false;
                    }
                    if (success)
                    {
                        transaction.Commit();
                        _logger.LogInformation("Removed the GeologicalPropertiesInterpolationCase of given ID from the GeologicalPropertiesInterpolationCaseTable successfully");
                    }
                    else
                    {
                        transaction.Rollback();
                    }
                    return success;
                }
                else
                {
                    _logger.LogWarning("Impossible to access the SQLite database");
                }
            }
            else
            {
                _logger.LogWarning("The GeologicalPropertiesInterpolationCase ID is null or empty");
            }
            return false;
        }
    }
}