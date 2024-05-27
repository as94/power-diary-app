namespace PowerDiary.Models;

public sealed class SomeoneEnteredRoom : IChatEvent
{
    public SomeoneEnteredRoom(Guid eventId, Guid userId, Guid roomId, DateTime createdAt, DateTime createdAtUtc)
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
}