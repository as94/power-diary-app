namespace PowerDiary.Domain.ChatEvents;

public sealed class UserLeftComment : IChatEvent
{
    public UserLeftComment(Guid eventId, Guid userId, Guid roomId, string comment, DateTime createdAt, DateTime createdAtUtc)
    {
        EventId = eventId;
        UserId = userId;
        RoomId = roomId;
        Comment = comment;
        CreatedAt = createdAt;
        CreatedAtUtc = createdAtUtc;
    }

    public Guid EventId { get; }
    public Guid UserId { get; }
    public Guid RoomId { get; }
    public string Comment { get; }
    public DateTime CreatedAt { get; }
    public DateTime CreatedAtUtc { get; }
    
    public string GetHighGranularityReportString(Dictionary<Guid, string> userNamesById)
    {
        var userName = userNamesById.GetValueOrDefault(
            UserId, 
            "Undefined user");
        
        return $"{userName} comments: \"{Comment}\"";
    }
    
    public string GetLowGranularityReportString(int count) => $"{count} comments";
    
    public IEnumerable<string> GetAggregatedReportStrings(IEnumerable<IChatEvent> events)
    {
        return new[] { GetLowGranularityReportString(events.Count()) };
    }
}