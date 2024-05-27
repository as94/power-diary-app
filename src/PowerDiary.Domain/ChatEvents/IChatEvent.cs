namespace PowerDiary.Domain.ChatEvents;

public interface IChatEvent
{
    public Guid EventId { get; }
    public Guid UserId { get; }
    public Guid RoomId { get; }
    public DateTime CreatedAt { get; }
    public DateTime CreatedAtUtc { get; }

    public string GetLowGranularityReportString(Dictionary<Guid, string> userNamesById);
    public string GetHighGranularityReportFormat { get; }
}