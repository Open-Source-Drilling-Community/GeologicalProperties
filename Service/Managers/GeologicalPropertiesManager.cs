using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using OSDC.DotnetLibraries.General.DataManagement;
using Microsoft.Data.Sqlite;
using System.Text.Json;

namespace GeologicalProperties.Service.Managers
{
    /// <summary>
    /// A manager for GeologicalProperties. The manager implements the singleton pattern as defined by 
    /// Gamma, Erich, et al. "Design patterns: Abstraction and reuse of object-oriented design." 
    /// European Conference on Object-Oriented Programming. Springer, Berlin, Heidelberg, 1993.
    /// </summary>
    public class GeologicalPropertiesManager
    {
        private static GeologicalPropertiesManager? _instance = null;
        private readonly ILogger<GeologicalPropertiesManager> _logger;
        private readonly SqlConnectionManager _connectionManager;

        private GeologicalPropertiesManager(ILogger<GeologicalPropertiesManager> logger, SqlConnectionManager connectionManager)
        {
            _logger = logger;
            _connectionManager = connectionManager;
        }

        internal static GeologicalPropertiesManager? Instance
        {
            get { return _instance; }
        }

        public static GeologicalPropertiesManager GetInstance(ILogger<GeologicalPropertiesManager> logger, SqlConnectionManager connectionManager)
        {
            _instance ??= new GeologicalPropertiesManager(logger, connectionManager);
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
                    command.CommandText = "SELECT COUNT(*) FROM GeologicalPropertiesTable";
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
                        _logger.LogError(ex, "Impossible to count records in the GeologicalPropertiesTable");
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
                    command.CommandText = "DELETE FROM GeologicalPropertiesTable";
                    command.ExecuteNonQuery();

                    transaction.Commit();
                    success = true;
                }
                catch (SqliteException ex)
                {
                    transaction.Rollback();
                    _logger.LogError(ex, "Impossible to clear the GeologicalPropertiesTable");
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
                command.CommandText = $"SELECT COUNT(*) FROM GeologicalPropertiesTable WHERE ID = '{guid}'";
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
                    _logger.LogError(ex, "Impossible to count rows from GeologicalPropertiesTable");
                }
            }
            else
            {
                _logger.LogWarning("Impossible to access the SQLite database");
            }
            return count >= 1;
        }

        /// <summary>
        /// Returns the list of Guid of all GeologicalProperties present in the microservice database 
        /// </summary>
        /// <returns>the list of Guid of all GeologicalProperties present in the microservice database</returns>
        public List<Guid>? GetAllGeologicalPropertiesId()
        {
            List<Guid> ids = [];
            var connection = _connectionManager.GetConnection();
            if (connection != null)
            {
                var command = connection.CreateCommand();
                command.CommandText = "SELECT ID FROM GeologicalPropertiesTable";
                try
                {
                    using var reader = command.ExecuteReader();
                    while (reader.Read() && !reader.IsDBNull(0))
                    {
                        Guid id = reader.GetGuid(0);
                        ids.Add(id);
                    }
                    _logger.LogInformation("Returning the list of ID of existing records from GeologicalPropertiesTable");
                    return ids;
                }
                catch (SqliteException ex)
                {
                    _logger.LogError(ex, "Impossible to get IDs from GeologicalPropertiesTable");
                }
            }
            else
            {
                _logger.LogWarning("Impossible to access the SQLite database");
            }
            return null;
        }

        /// <summary>
        /// Returns the list of MetaInfo of all GeologicalProperties present in the microservice database 
        /// </summary>
        /// <returns>the list of MetaInfo of all GeologicalProperties present in the microservice database</returns>
        public List<MetaInfo?>? GetAllGeologicalPropertiesMetaInfo()
        {
            List<MetaInfo?> metaInfos = new();
            var connection = _connectionManager.GetConnection();
            if (connection != null)
            {
                var command = connection.CreateCommand();
                command.CommandText = "SELECT MetaInfo FROM GeologicalPropertiesTable";
                try
                {
                    using var reader = command.ExecuteReader();
                    while (reader.Read() && !reader.IsDBNull(0))
                    {
                        string mInfo = reader.GetString(0);
                        MetaInfo? metaInfo = JsonSerializer.Deserialize<MetaInfo>(mInfo, JsonSettings.Options);
                        metaInfos.Add(metaInfo);
                    }
                    _logger.LogInformation("Returning the list of MetaInfo of existing records from GeologicalPropertiesTable");
                    return metaInfos;
                }
                catch (SqliteException ex)
                {
                    _logger.LogError(ex, "Impossible to get IDs from GeologicalPropertiesTable");
                }
            }
            else
            {
                _logger.LogWarning("Impossible to access the SQLite database");
            }
            return null;
        }

        /// <summary>
        /// Returns the GeologicalProperties identified by its Guid from the microservice database 
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>the GeologicalProperties identified by its Guid from the microservice database</returns>
        public Model.GeologicalProperties? GetGeologicalPropertiesById(Guid guid)
        {
            if (!guid.Equals(Guid.Empty))
            {
                var connection = _connectionManager.GetConnection();
                if (connection != null)
                {
                    Model.GeologicalProperties? geologicalProperties;
                    var command = connection.CreateCommand();
                    command.CommandText = $"SELECT GeologicalProperties FROM GeologicalPropertiesTable WHERE ID = '{guid}'";
                    try
                    {
                        using var reader = command.ExecuteReader();
                        if (reader.Read() && !reader.IsDBNull(0))
                        {
                            string data = reader.GetString(0);
                            geologicalProperties = JsonSerializer.Deserialize<Model.GeologicalProperties>(data, JsonSettings.Options);
                            if (geologicalProperties != null && geologicalProperties.MetaInfo != null && !geologicalProperties.MetaInfo.ID.Equals(guid))
                                throw new SqliteException("SQLite database corrupted: returned GeologicalProperties is null or has been jsonified with the wrong ID.", 1);
                        }
                        else
                        {
                            _logger.LogInformation("No GeologicalProperties of given ID in the database");
                            return null;
                        }
                    }
                    catch (SqliteException ex)
                    {
                        _logger.LogError(ex, "Impossible to get the GeologicalProperties with the given ID from GeologicalPropertiesTable");
                        return null;
                    }
                    _logger.LogInformation("Returning the GeologicalProperties of given ID from GeologicalPropertiesTable");
                    return geologicalProperties;
                }
                else
                {
                    _logger.LogWarning("Impossible to access the SQLite database");
                }
            }
            else
            {
                _logger.LogWarning("The given GeologicalProperties ID is null or empty");
            }
            return null;
        }

        /// <summary>
        /// Returns the list of all GeologicalProperties present in the microservice database 
        /// </summary>
        /// <returns>the list of all GeologicalProperties present in the microservice database</returns>
        public List<Model.GeologicalProperties?>? GetAllGeologicalProperties()
        {
            List<Model.GeologicalProperties?> vals = [];
            var connection = _connectionManager.GetConnection();
            if (connection != null)
            {
                var command = connection.CreateCommand();
                command.CommandText = "SELECT GeologicalProperties FROM GeologicalPropertiesTable";
                try
                {
                    using var reader = command.ExecuteReader();
                    while (reader.Read() && !reader.IsDBNull(0))
                    {
                        string data = reader.GetString(0);
                        Model.GeologicalProperties? geologicalProperties = JsonSerializer.Deserialize<Model.GeologicalProperties>(data, JsonSettings.Options);
                        vals.Add(geologicalProperties);
                    }
                    _logger.LogInformation("Returning the list of existing GeologicalProperties from GeologicalPropertiesTable");
                    return vals;
                }
                catch (SqliteException ex)
                {
                    _logger.LogError(ex, "Impossible to get GeologicalProperties from GeologicalPropertiesTable");
                }
            }
            else
            {
                _logger.LogWarning("Impossible to access the SQLite database");
            }
            return null;
        }

        /// <summary>
        /// Returns the list of all GeologicalPropertiesLight present in the microservice database 
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>the list of GeologicalPropertiesLight present in the microservice database</returns>
        public List<Model.GeologicalPropertiesLight>? GetAllGeologicalPropertiesLight()
        {
            List<Model.GeologicalPropertiesLight>? geologicalPropertiesLightList = [];
            var connection = _connectionManager.GetConnection();
            if (connection != null)
            {
                var command = connection.CreateCommand();
                command.CommandText = "SELECT MetaInfo, Name, Description, CreationDate, LastModificationDate, WellBoreID, TrajectoryID, IsPrognosed FROM GeologicalPropertiesTable";
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
                        Guid? wellBoreID = null;
                        if (Guid.TryParse(reader.GetString(5), out Guid wbID))
                        {
                            wellBoreID = wbID;
                        }
                        Guid? trajectoryID = null;
                        if (Guid.TryParse(reader.GetString(6), out Guid trajID))
                        {
                            trajectoryID = trajID;
                        }
                        bool isPrognosed = reader.GetBoolean(7);
                        geologicalPropertiesLightList.Add(new Model.GeologicalPropertiesLight(
                                metaInfo,
                                string.IsNullOrEmpty(name) ? null : name,
                                string.IsNullOrEmpty(descr) ? null : descr,
                                creationDate,
                                lastModificationDate,
                                wellBoreID,
                                trajectoryID,
                                isPrognosed));
                    }
                    _logger.LogInformation("Returning the list of existing GeologicalPropertiesLight from GeologicalPropertiesTable");
                    return geologicalPropertiesLightList;
                }
                catch (SqliteException ex)
                {
                    _logger.LogError(ex, "Impossible to get light datas from GeologicalPropertiesTable");
                }
            }
            else
            {
                _logger.LogWarning("Impossible to access the SQLite database");
            }
            return null;
        }

        /// <summary>
        /// Adds the given GeologicalProperties in the microservice database
        /// </summary>
        /// <param name="geologicalProperties"></param>
        /// <returns>true if the given GeologicalProperties has been added successfully to the microservice database</returns>
        public bool AddGeologicalProperties(Model.GeologicalProperties? geologicalProperties)
        {
            if (geologicalProperties != null && geologicalProperties.MetaInfo != null && geologicalProperties.MetaInfo.ID != Guid.Empty)
            {
                //if successful, check if another parent data with the same ID was calculated/added during the calculation time
                Model.GeologicalProperties? newGeologicalProperties = GetGeologicalPropertiesById(geologicalProperties.MetaInfo.ID);
                if (newGeologicalProperties == null)
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
                            string metaInfo = JsonSerializer.Serialize(geologicalProperties.MetaInfo, JsonSettings.Options);
                            string? cDate = null;
                            if (geologicalProperties.CreationDate != null)
                                cDate = ((DateTimeOffset)geologicalProperties.CreationDate).ToString(SqlConnectionManager.DATE_TIME_FORMAT);
                            string? lDate = null;
                            if (geologicalProperties.LastModificationDate != null)
                                lDate = ((DateTimeOffset)geologicalProperties.LastModificationDate).ToString(SqlConnectionManager.DATE_TIME_FORMAT);
                            string data = JsonSerializer.Serialize(geologicalProperties, JsonSettings.Options);
                            var command = connection.CreateCommand();
                            command.CommandText = "INSERT INTO GeologicalPropertiesTable (" +
                                "ID, " +
                                "MetaInfo, " +
                                "Name, " +
                                "Description, " +
                                "CreationDate, " +
                                "LastModificationDate, " +
                                "WellBoreID, " +
                                "TrajectoryID, " +
                                "IsPrognosed, " +
                                "GeologicalProperties" +
                                ") VALUES (" +
                                $"'{geologicalProperties.MetaInfo.ID}', " +
                                $"'{metaInfo}', " +
                                $"'{geologicalProperties.Name}', " +
                                $"'{geologicalProperties.Description}', " +
                                $"'{cDate}', " +
                                $"'{lDate}', " +
                                $"'{geologicalProperties.WellBoreID}', " +
                                $"'{geologicalProperties.TrajectoryID}', " +
                                $"'{geologicalProperties.IsPrognosed}', " +
                                $"'{data}'" +
                                ")";
                            int count = command.ExecuteNonQuery();
                            if (count != 1)
                            {
                                _logger.LogWarning("Impossible to insert the given GeologicalProperties into the GeologicalPropertiesTable");
                                success = false;
                            }
                        }
                        catch (SqliteException ex)
                        {
                            _logger.LogError(ex, "Impossible to add the given GeologicalProperties into GeologicalPropertiesTable");
                            success = false;
                        }
                        //finalizing SQL transaction
                        if (success)
                        {
                            transaction.Commit();
                            _logger.LogInformation("Added the given GeologicalProperties of given ID into the GeologicalPropertiesTable successfully");
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
                    _logger.LogWarning("Impossible to post GeologicalProperties. ID already found in database.");
                    return false;
                }

            }
            else
            {
                _logger.LogWarning("The GeologicalProperties ID or the ID of its input are null or empty");
            }
            return false;
        }

        /// <summary>
        /// Performs calculation on the given GeologicalProperties and updates it in the microservice database
        /// </summary>
        /// <param name="geologicalProperties"></param>
        /// <returns>true if the given GeologicalProperties has been updated successfully</returns>
        public bool UpdateGeologicalPropertiesById(Guid guid, Model.GeologicalProperties? geologicalProperties)
        {
            bool success = true;
            if (guid != Guid.Empty && geologicalProperties != null && geologicalProperties.MetaInfo != null && geologicalProperties.MetaInfo.ID == guid)
            {
                //update GeologicalPropertiesTable
                var connection = _connectionManager.GetConnection();
                if (connection != null)
                {
                    using SqliteTransaction transaction = connection.BeginTransaction();
                    //update fields in GeologicalPropertiesTable
                    try
                    {
                        string metaInfo = JsonSerializer.Serialize(geologicalProperties.MetaInfo, JsonSettings.Options);
                        string? cDate = null;
                        if (geologicalProperties.CreationDate != null)
                            cDate = ((DateTimeOffset)geologicalProperties.CreationDate).ToString(SqlConnectionManager.DATE_TIME_FORMAT);
                        geologicalProperties.LastModificationDate = DateTimeOffset.UtcNow;
                        string? lDate = ((DateTimeOffset)geologicalProperties.LastModificationDate).ToString(SqlConnectionManager.DATE_TIME_FORMAT);
                        string data = JsonSerializer.Serialize(geologicalProperties, JsonSettings.Options);
                        var command = connection.CreateCommand();
                        command.CommandText = $"UPDATE GeologicalPropertiesTable SET " +
                            $"MetaInfo = '{metaInfo}', " +
                            $"Name = '{geologicalProperties.Name}', " +
                            $"Description = '{geologicalProperties.Description}', " +
                            $"CreationDate = '{cDate}', " +
                            $"LastModificationDate = '{lDate}', " +
                            $"WellBoreID = '{geologicalProperties.WellBoreID}', " +
                            $"TrajectoryID = '{geologicalProperties.TrajectoryID}', " +
                            $"IsPrognosed = '{geologicalProperties.IsPrognosed}', " +
                            $"GeologicalProperties = '{data}' " +
                            $"WHERE ID = '{guid}'";
                        int count = command.ExecuteNonQuery();
                        if (count != 1)
                        {
                            _logger.LogWarning("Impossible to update the GeologicalProperties");
                            success = false;
                        }
                    }
                    catch (SqliteException ex)
                    {
                        _logger.LogError(ex, "Impossible to update the GeologicalProperties");
                        success = false;
                    }

                    // Finalizing
                    if (success)
                    {
                        transaction.Commit();
                        _logger.LogInformation("Updated the given GeologicalProperties successfully");
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
                _logger.LogWarning("The GeologicalProperties ID or the ID of some of its attributes are null or empty");
            }
            return false;
        }

        /// <summary>
        /// Deletes the GeologicalProperties of given ID from the microservice database
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>true if the GeologicalProperties was deleted from the microservice database</returns>
        public bool DeleteGeologicalPropertiesById(Guid guid)
        {
            if (!guid.Equals(Guid.Empty))
            {
                var connection = _connectionManager.GetConnection();
                if (connection != null)
                {
                    using var transaction = connection.BeginTransaction();
                    bool success = true;
                    //delete GeologicalProperties from GeologicalPropertiesTable
                    try
                    {
                        var command = connection.CreateCommand();
                        command.CommandText = $"DELETE FROM GeologicalPropertiesTable WHERE ID = '{guid}'";
                        int count = command.ExecuteNonQuery();
                        if (count < 0)
                        {
                            _logger.LogWarning("Impossible to delete the GeologicalProperties of given ID from the GeologicalPropertiesTable");
                            success = false;
                        }
                    }
                    catch (SqliteException ex)
                    {
                        _logger.LogError(ex, "Impossible to delete the GeologicalProperties of given ID from GeologicalPropertiesTable");
                        success = false;
                    }
                    if (success)
                    {
                        transaction.Commit();
                        _logger.LogInformation("Removed the GeologicalProperties of given ID from the GeologicalPropertiesTable successfully");
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
                _logger.LogWarning("The GeologicalProperties ID is null or empty");
            }
            return false;
        }
    }
}