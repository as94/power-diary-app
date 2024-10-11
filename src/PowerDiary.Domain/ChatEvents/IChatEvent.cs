namespace PowerDiary.Domain.ChatEvents;

public interface IChatEvent
{
    public Guid EventId { get; }
    public Guid UserId { get; }
    public Guid RoomId { get; }
    public DateTime CreatedAt { get; }
    public DateTime CreatedAtUtc { get; }

    public string GetHighGranularityReportString(Dictionary<Guid, string> userNamesById);
    public string GetLowGranularityReportString(int count);

    public IEnumerable<string> GetAggregatedReportStrings(IEnumerable<IChatEvent> events);
}