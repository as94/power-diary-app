using PowerDiary.Domain.ChatEvents;

namespace PowerDiary.Tests.DummyData;

public static class ChatEventsData
{
    public static IChatEvent[] GetBunchOfEvents(
        Guid bobId,
        Guid kateId,
        Guid roomId,
        DateTime initialDateTime)
    {
        var bobEntered = new UserEnteredRoom(
            Guid.NewGuid(),
            bobId,
            roomId,
            initialDateTime,
            initialDateTime);
        
        var after5Minutes = initialDateTime.AddMinutes(5);
        var kateEntered = new UserEnteredRoom(
            Guid.NewGuid(),
            kateId,
            roomId,
            after5Minutes,
            after5Minutes);
        
        var after15Minutes = initialDateTime.AddMinutes(15);
        var bobLeftComment = new UserLeftComment(
            Guid.NewGuid(),
            bobId,
            roomId,
            "Hey, Kate - high five?",
            after15Minutes,
            after15Minutes);
        
        var at5_17 = initialDateTime.AddMinutes(17);
        var kateHighFiveBob = new UserGaveHighFive(
            Guid.NewGuid(),
            kateId,
            roomId,
            bobId,
            at5_17,
            at5_17);
        
        var after18Minutes = initialDateTime.AddMinutes(18);
        var bobLeftRoom = new UserLeftRoom(
            Guid.NewGuid(),
            bobId,
            roomId,
            after18Minutes,
            after18Minutes);
        
        var after20Minutes = initialDateTime.AddMinutes(20);
        var kateLeftComment = new UserLeftComment(
            Guid.NewGuid(),
            kateId,
            roomId,
            "Oh, typical",
            after20Minutes,
            after20Minutes);
        
        var after21Minutes = initialDateTime.AddMinutes(21);
        var kateLeftRoom = new UserLeftRoom(
            Guid.NewGuid(),
            kateId,
            roomId,
            after21Minutes,
            after21Minutes);

        return
        [
            bobEntered,
            kateEntered,
            bobLeftComment,
            kateHighFiveBob,
            bobLeftRoom,
            kateLeftComment,
            kateLeftRoom
        ];
    }
}