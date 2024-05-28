using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using PowerDiary.Domain;
using PowerDiary.Domain.ChatEvents;

namespace PowerDiary.Data;

internal sealed class SqlChatEventsRepository : IChatEventsRepository
{
    private readonly string _connectionString;

    public SqlChatEventsRepository(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public async Task AddRangeAsync(IEnumerable<IChatEvent> events, CancellationToken ct)
    {
        if (events == null)
        {
            throw new ArgumentNullException(nameof(events));
        }

        var eventsArray = events as IChatEvent[] ?? events.ToArray();
        if (!eventsArray.Any())
        {
            return;
        }
        
        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync(ct);

        await using var transaction = await connection.BeginTransactionAsync(ct);
        try
        {
            foreach (var @event in eventsArray)
            {
                await AddEventAsync(@event, connection, transaction, ct);
            }
                    
            await transaction.CommitAsync(ct);
        }
        catch
        {
            await transaction.RollbackAsync(ct);
            throw;
        }
    }
    
    private async Task AddEventAsync(IChatEvent @event, MySqlConnection connection, MySqlTransaction transaction, CancellationToken ct)
    {
        await using var command = connection.CreateCommand();
        command.Transaction = transaction;
        
        command.CommandText = @"
            INSERT INTO ChatEvents (EventId, UserId, RoomId, EventType, CreatedAt, CreatedAtUtc, EventData)
            VALUES (@EventId, @UserId, @RoomId, @EventType, @CreatedAt, @CreatedAtUtc, @EventData)";
        
        command.Parameters.AddWithValue("@EventId", @event.EventId.ToByteArray());
        command.Parameters.AddWithValue("@UserId", @event.UserId.ToByteArray());
        command.Parameters.AddWithValue("@RoomId", @event.RoomId.ToByteArray());
        command.Parameters.AddWithValue("@EventType", @event.GetType().FullName);
        command.Parameters.AddWithValue("@CreatedAt", @event.CreatedAt);
        command.Parameters.AddWithValue("@CreatedAtUtc", @event.CreatedAtUtc);
        command.Parameters.AddWithValue("@EventData", JsonConvert.SerializeObject(@event));

        await command.ExecuteNonQueryAsync(ct);
    }

    public async Task<IEnumerable<IChatEvent>> GetEventsAsync(DateTime from, DateTime to, CancellationToken ct)
    {
        if (from > to)
        {
            return [];
        }
        
        var events = new List<IChatEvent>();

        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync(ct);

        await using var command = connection.CreateCommand();
        command.CommandText = @"
                SELECT EventData, EventType
                FROM ChatEvents
                WHERE CreatedAtUtc >= @From AND CreatedAtUtc <= @To";
        command.Parameters.AddWithValue("@From", from);
        command.Parameters.AddWithValue("@To", to);

        await using var reader = await command.ExecuteReaderAsync(ct);
        while (await reader.ReadAsync(ct))
        {
            var eventType = reader.GetString(reader.GetOrdinal("EventType"));
            var eventDataJson = reader.GetString(reader.GetOrdinal("EventData"));

            var type = typeof(IChatEvent).Assembly.GetType(eventType);
            
            if (type != null)
            {
                var chatEvent = (IChatEvent)JsonConvert.DeserializeObject(eventDataJson, type)!;
                events.Add(chatEvent);
            }
        }

        return events;
    }
}