namespace PowerDiary.Models;

public sealed class SomeoneGaveHighFive : IChatEvent
{
    public SomeoneGaveHighFive(Guid eventId, Guid userId, Guid roomId, Guid[] recipientIds, DateTime createdAt, DateTime createdAtUtc)
    {
        EventId = eventId;
        UserId = userId;
        RoomId = roomId;
        RecipientIds = recipientIds;
        CreatedAt = createdAt;
        CreatedAtUtc = createdAtUtc;
    }
    
    public SomeoneGaveHighFive(Guid eventId, Guid userId, Guid roomId, DateTime createdAt, DateTime createdAtUtc)
        : this(eventId, userId, roomId, [], createdAt, createdAtUtc)
    {
    }

    public Guid EventId { get; }
    public Guid UserId { get; }
    public Guid RoomId { get; }
    public Guid[] RecipientIds { get; }
    public DateTime CreatedAt { get; }
    public DateTime CreatedAtUtc { get; }
}