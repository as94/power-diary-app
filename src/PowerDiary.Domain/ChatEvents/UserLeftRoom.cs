namespace PowerDiary.Domain.ChatEvents;

public sealed class UserLeftRoom : IChatEvent
{
    public UserLeftRoom(Guid eventId, Guid userId, Guid roomId, DateTime createdAt, DateTime createdAtUtc)
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
    
    public string GetHighGranularityReportString(Dictionary<Guid, string> userNamesById)
    {
        var userName = userNamesById.GetValueOrDefault(
            UserId, 
            "Undefined user");
        
        return $"{userName} leaves";
    }
    
    public string GetLowGranularityReportString(int count) => $"{count} left";
}