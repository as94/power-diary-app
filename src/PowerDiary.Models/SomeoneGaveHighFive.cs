namespace PowerDiary.Models;

public sealed class SomeoneGaveHighFive : IChatEvent
{
    public SomeoneGaveHighFive(Guid eventId, Guid userId, Guid roomId, Guid[] recipientIds)
    {
        EventId = eventId;
        UserId = userId;
        RoomId = roomId;
        RecipientIds = recipientIds;
    }
    
    public SomeoneGaveHighFive(Guid eventId, Guid userId, Guid roomId)
        : this(eventId, userId, roomId, [])
    {
    }

    public Guid EventId { get; }
    public Guid UserId { get; }
    public Guid RoomId { get; }
    public Guid[] RecipientIds { get; }
}