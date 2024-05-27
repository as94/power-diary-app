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
}