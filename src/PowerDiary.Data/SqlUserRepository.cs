using System.Data;
using MySql.Data.MySqlClient;
using PowerDiary.Domain;
using PowerDiary.Domain.Models;

namespace PowerDiary.Data;

internal sealed class SqlUserRepository : IUserRepository
{
    private readonly string _connectionString;

    public SqlUserRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task AddRangeAsync(IEnumerable<User> users, CancellationToken ct)
    {
        if (users == null)
        {
            throw new ArgumentNullException(nameof(users));
        }

        var usersArray = users as User[] ?? users.ToArray();
        if (!usersArray.Any())
        {
            return;
        }

        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync(ct);

        await using var transaction = await connection.BeginTransactionAsync(ct);
        try
        {
            foreach (var user in usersArray)
            {
                await AddUserAsync(user, connection, transaction, ct);
            }
                    
            await transaction.CommitAsync(ct);
        }
        catch
        {
            await transaction.RollbackAsync(ct);
            throw;
        }
    }

    private async Task AddUserAsync(User user, MySqlConnection connection, MySqlTransaction transaction, CancellationToken ct)
    {
        await using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = @"
                INSERT INTO Users (Id, Name)
                VALUES (@Id, @Name)";
        command.Parameters.AddWithValue("@Id", user.Id.ToByteArray());
        command.Parameters.AddWithValue("@Name", user.Name);

        await command.ExecuteNonQueryAsync(ct);
    }

    public async Task<IEnumerable<User>> GetRangeAsync(IEnumerable<Guid> userIds, CancellationToken ct)
    {
        if (userIds == null)
        {
            throw new ArgumentNullException(nameof(userIds));
        }

        var userIdsArray = userIds as Guid[] ?? userIds.ToArray();
        if (!userIdsArray.Any())
        {
            return [];
        }

        var users = new List<User>();

        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync(ct);

        foreach (var userId in userIdsArray)
        {
            var user = await GetUserAsync(userId, connection, ct);
            if (user != null)
            {
                users.Add(user);
            }
        }

        return users;
    }

    private async Task<User?> GetUserAsync(Guid userId, MySqlConnection connection, CancellationToken ct)
    {
        await using var command = connection.CreateCommand();
        command.CommandText = @"
                SELECT Id, Name
                FROM Users
                WHERE Id = @Id";
        command.Parameters.AddWithValue("@Id", userId.ToByteArray());

        await using var reader = await command.ExecuteReaderAsync(ct);
        if (await reader.ReadAsync(ct))
        {
            return new User(
                reader.GetGuid("Id"),
                reader.GetString("Name"));
        }

        return null;
    }
}