namespace PowerDiary.Models;

public interface IChatEvent
{
    public Guid EventId { get; }
    public Guid UserId { get; }
    public Guid RoomId { get; }
}