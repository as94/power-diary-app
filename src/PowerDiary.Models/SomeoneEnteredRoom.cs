namespace PowerDiary.Models;

public sealed class SomeoneEnteredRoom : IChatEvent
{
    public SomeoneEnteredRoom(Guid eventId, Guid userId, Guid roomId)
    {
        EventId = eventId;
        UserId = userId;
        RoomId = roomId;
    }

    public Guid EventId { get; }
    public Guid UserId { get; }
    public Guid RoomId { get; }
}