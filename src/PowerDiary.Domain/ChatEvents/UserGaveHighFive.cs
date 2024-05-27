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
}