namespace PowerDiary.Domain.ChatEvents;

public sealed class UserEnteredRoom : IChatEvent
{
    public UserEnteredRoom(Guid eventId, Guid userId, Guid roomId, DateTime createdAt, DateTime createdAtUtc)
    {
        EventId = eventId;
        UserId = userId;
        RoomId = roomId;
        CreatedAt = createdAt;
        CreatedAtUtc = createdAtUtc;
    }

    public Guid EventId { get; }
    public Guid UserId { get; }
    public Guid RoomId { get; }
    public DateTime CreatedAt { get; }
    public DateTime CreatedAtUtc { get; }
    
    public string GetLowGranularityReportString(Dictionary<Guid, string> userNamesById)
    {
        var userName = userNamesById.GetValueOrDefault(
            UserId, 
            "Undefined user");
        
        return $"{userName} enters the room";
    }

    public string GetHighGranularityReportFormat => "{0} person entered";
}