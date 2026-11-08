
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace RobotPlanet.Core
{
    public class LocalDbWorldStorage : IWorldStorage
    {
        // Ühendus SQL Server LocalDB-ga
        private readonly string masterConnection =
            @"Server=(localdb)\MSSQLLocalDB;" +
            "Database=master;" +
            "Integrated Security=True;" +
            "TrustServerCertificate=True;";

        private readonly string worldConnection =
            @"Server=(localdb)\MSSQLLocalDB;" +
            "Database=RobotPlanetDb;" +
            "Integrated Security=True;" +
            "TrustServerCertificate=True;";

        // Loon andmebaasi ja tabelid
        private async Task InitializeAsync(CancellationToken token)
        {
            await using (SqlConnection connection =
                new SqlConnection(masterConnection))
            {
                await connection.OpenAsync(token);

                string sql = """
                    IF DB_ID('RobotPlanetDb') IS NULL
                        CREATE DATABASE RobotPlanetDb;
                    """;

                await using SqlCommand command =
                    new SqlCommand(sql, connection);

                await command.ExecuteNonQueryAsync(token);
            }

            await using (SqlConnection connection =
                new SqlConnection(worldConnection))
            {
                await connection.OpenAsync(token);

                string sql = """
                    IF OBJECT_ID('RobotGroups', 'U') IS NULL
                    CREATE TABLE RobotGroups
                    (
                        GroupId INT PRIMARY KEY IDENTITY(1,1),
                        GroupName NVARCHAR(100) NOT NULL UNIQUE
                    );

                    IF OBJECT_ID('Robots', 'U') IS NULL
                    CREATE TABLE Robots
                    (
                        Id UNIQUEIDENTIFIER PRIMARY KEY,
                        RobotType NVARCHAR(50) NOT NULL,
                        RobotName NVARCHAR(100) NOT NULL,
                        Energy INT NOT NULL
                            CHECK (Energy BETWEEN 0 AND 100),
                        GroupId INT NOT NULL,
                        FOREIGN KEY (GroupId)
                            REFERENCES RobotGroups(GroupId)
                    );
                    """;

                await using SqlCommand command =
                    new SqlCommand(sql, connection);

                await command.ExecuteNonQueryAsync(token);
            }
        }

        // Salvestan kogu maailma andmebaasi
        public async Task SaveAsync(
            WorldDto world,
            string path,
            CancellationToken token = default)
        {
            if (world == null || world.Robots == null)
                throw new ArgumentException("Maailma andmed puuduvad.");

            // Kontrollin andmeid enne salvestamist
            WorldMapper.FromDto(world);

            await InitializeAsync(token);

            await using SqlConnection connection =
                new SqlConnection(worldConnection);

            await connection.OpenAsync(token);

            using SqlTransaction transaction =
                (SqlTransaction)await connection.BeginTransactionAsync(token);

            try
            {
                // Kustutan vana maailma andmed
                await using (SqlCommand deleteRobots =
                    new SqlCommand(
                        "DELETE FROM Robots",
                        connection, transaction))
                {
                    await deleteRobots.ExecuteNonQueryAsync(token);
                }

                await using (SqlCommand deleteGroups =
                    new SqlCommand(
                        "DELETE FROM RobotGroups",
                        connection, transaction))
                {
                    await deleteGroups.ExecuteNonQueryAsync(token);
                }

                // Salvestan iga roboti koos grupiga
                foreach (RobotDto robot in world.Robots)
                {
                    string groupSql = """
                        IF NOT EXISTS (
                            SELECT 1 FROM RobotGroups
                            WHERE GroupName = @name
                        )
                            INSERT INTO RobotGroups (GroupName)
                            VALUES (@name);

                        SELECT GroupId FROM RobotGroups
                        WHERE GroupName = @name;
                        """;

                    await using SqlCommand groupCommand =
                        new SqlCommand(
                            groupSql, connection, transaction);

                    groupCommand.Parameters.Add(
                        "@name", SqlDbType.NVarChar, 100)
                        .Value = robot.GroupName;

                    object? groupResult =
                        await groupCommand.ExecuteScalarAsync(token);

                    int groupId = Convert.ToInt32(groupResult);

                    string robotSql = """
                        INSERT INTO Robots
                        (Id, RobotType, RobotName, Energy, GroupId)
                        VALUES
                        (@id, @type, @name, @energy, @groupId);
                        """;

                    await using SqlCommand robotCommand =
                        new SqlCommand(
                            robotSql, connection, transaction);

                    robotCommand.Parameters.Add(
                        "@id", SqlDbType.UniqueIdentifier)
                        .Value = robot.Id;

                    robotCommand.Parameters.Add(
                        "@type", SqlDbType.NVarChar, 50)
                        .Value = robot.Type;

                    robotCommand.Parameters.Add(
                        "@name", SqlDbType.NVarChar, 100)
                        .Value = robot.Name;

                    robotCommand.Parameters.Add(
                        "@energy", SqlDbType.Int)
                        .Value = robot.Energy;

                    robotCommand.Parameters.Add(
                        "@groupId", SqlDbType.Int)
                        .Value = groupId;

                    await robotCommand.ExecuteNonQueryAsync(token);
                }

                await transaction.CommitAsync(token);
            }
            catch
            {
                await transaction.RollbackAsync(CancellationToken.None);
                throw;
            }
        }

        // Laen maailma andmebaasist
        public async Task<WorldDto> LoadAsync(
            string path,
            CancellationToken token = default)
        {
            await InitializeAsync(token);

            WorldDto world = new WorldDto();

            await using SqlConnection connection =
                new SqlConnection(worldConnection);

            await connection.OpenAsync(token);

            string sql = """
                SELECT
                    r.Id,
                    r.RobotType,
                    r.RobotName,
                    r.Energy,
                    g.GroupName
                FROM Robots r
                INNER JOIN RobotGroups g
                    ON r.GroupId = g.GroupId
                ORDER BY r.RobotName;
                """;

            await using SqlCommand command =
                new SqlCommand(sql, connection);

            await using SqlDataReader reader =
                await command.ExecuteReaderAsync(token);

            while (await reader.ReadAsync(token))
            {
                RobotDto robot = new RobotDto
                {
                    Id = reader.GetGuid(0),
                    Type = reader.GetString(1),
                    Name = reader.GetString(2),
                    Energy = reader.GetInt32(3),
                    GroupName = reader.GetString(4)
                };

                world.Robots.Add(robot);
            }

            // Kontrollin taastatud maailma
            WorldMapper.FromDto(world);

            return world;
        }
    }
}
