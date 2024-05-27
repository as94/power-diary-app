namespace PowerDiary.Models;

public sealed class SomeoneLeftComment : IChatEvent
{
    public SomeoneLeftComment(Guid eventId, Guid userId, Guid roomId, string comment)
    {
        EventId = eventId;
        UserId = userId;
        RoomId = roomId;
        Comment = comment;
    }

    public Guid EventId { get; }
    public Guid UserId { get; }
    public Guid RoomId { get; }
    public string Comment { get; }
}