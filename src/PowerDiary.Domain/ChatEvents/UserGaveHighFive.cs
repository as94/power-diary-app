namespace PowerDiary.Domain.ChatEvents;

public sealed class UserGaveHighFive : IChatEvent
{
    public UserGaveHighFive(Guid eventId, Guid userId, Guid roomId, Guid recipientId, DateTime createdAt, DateTime createdAtUtc)
    {
        EventId = eventId;
        UserId = userId;
        RoomId = roomId;
        RecipientId = recipientId;
        CreatedAt = createdAt;
        CreatedAtUtc = createdAtUtc;
    }

    public Guid EventId { get; }
    public Guid UserId { get; }
    public Guid RoomId { get; }
    public Guid RecipientId { get; }
    public DateTime CreatedAt { get; }
    public DateTime CreatedAtUtc { get; }
    
    public string GetHighGranularityReportString(Dictionary<Guid, string> userNamesById)
    {
        var userName = userNamesById.GetValueOrDefault(
            UserId, 
            "Undefined user");
        
        var recipientName = userNamesById.GetValueOrDefault(
            RecipientId, 
            "Undefined recipient");
        
        return $"{userName} high-fives {recipientName}";
    }
    
    public string GetLowGranularityReportString(int count)
    {
        return count == 1
            ? $"1 person high-fived {count} other person"
            : $"1 person high-fived {count} other people";
    }
}