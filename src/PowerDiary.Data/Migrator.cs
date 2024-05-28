using MySql.Data.MySqlClient;

namespace PowerDiary.Data;

public sealed class Migrator
{
    private readonly string _connectionString;

    public Migrator(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public async Task RunAsync(CancellationToken ct)
    {
        string migrationsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Migrations");

        await using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync(ct);
            EnsureAppliedMigrationsTableExists(connection);

            var appliedMigrations = GetAppliedMigrations(connection);

            var migrationFiles = Directory.GetFiles(migrationsFolder, "*.sql");
            Array.Sort(migrationFiles);

            foreach (var file in migrationFiles)
            {
                var migrationId = Path.GetFileNameWithoutExtension(file);

                if (!appliedMigrations.Contains(migrationId))
                {
                    var script = await File.ReadAllTextAsync(file, ct);
                    await using (var command = new MySqlCommand(script, connection))
                    {
                        await command.ExecuteNonQueryAsync(ct);
                    }

                    LogMigration(connection, migrationId);
                    Console.WriteLine($"Applied migration: {migrationId}");
                }
            }
        }
        
        static void EnsureAppliedMigrationsTableExists(MySqlConnection connection)
        {
            var createTableQuery = @"
                CREATE TABLE IF NOT EXISTS AppliedMigrations (
                    MigrationId VARCHAR(255) PRIMARY KEY,
                    AppliedAt DATETIME DEFAULT CURRENT_TIMESTAMP
                )";

            using var command = new MySqlCommand(createTableQuery, connection);
            command.ExecuteNonQuery();
        }
        
        static HashSet<string> GetAppliedMigrations(MySqlConnection connection)
        {
            var appliedMigrations = new HashSet<string>();

            var query = "SELECT MigrationId FROM AppliedMigrations";
            using var command = new MySqlCommand(query, connection);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                appliedMigrations.Add(reader.GetString(0));
            }

            return appliedMigrations;
        }

        static void LogMigration(MySqlConnection connection, string migrationId)
        {
            var query = "INSERT INTO AppliedMigrations (MigrationId) VALUES (@MigrationId)";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@MigrationId", migrationId);
            command.ExecuteNonQuery();
        }
    }
}