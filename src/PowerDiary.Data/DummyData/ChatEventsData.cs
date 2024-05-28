using PowerDiary.Domain.ChatEvents;

namespace PowerDiary.Data.DummyData;

public static class ChatEventsData
{
    public static IChatEvent[] GetEventsForHighGranularity(
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

    public static IChatEvent[] GetEventsForLowGranularity(
        Guid bobId,
        Guid kateId,
        Guid roomId,
        DateTime initialDateTime)
    {
        var eventsAtFirstHour = new IChatEvent[]
        {
            new UserEnteredRoom(
                Guid.NewGuid(),
                bobId,
                roomId,
                initialDateTime.AddMinutes(2),
                initialDateTime.AddMinutes(2)),
            new UserLeftRoom(
                Guid.NewGuid(),
                kateId,
                roomId,
                initialDateTime.AddMinutes(20),
                initialDateTime.AddMinutes(20)),
            new UserLeftRoom(
                Guid.NewGuid(),
                bobId,
                roomId,
                initialDateTime.AddMinutes(35),
                initialDateTime.AddMinutes(35)),
            new UserGaveHighFive(
                Guid.NewGuid(),
                kateId,
                roomId,
                bobId,
                initialDateTime.AddMinutes(40),
                initialDateTime.AddMinutes(40)),
            new UserLeftComment(
                Guid.NewGuid(),
                bobId,
                roomId,
                "Some comment",
                initialDateTime.AddMinutes(50),
                initialDateTime.AddMinutes(50)),
            new UserLeftComment(
                Guid.NewGuid(),
                bobId,
                roomId,
                "Some comment",
                initialDateTime.AddMinutes(55),
                initialDateTime.AddMinutes(55))
        };

        var atSecondHour = initialDateTime.AddHours(1);

        var eventsAtSecondHour = new IChatEvent[]
        {
            new UserEnteredRoom(
                Guid.NewGuid(),
                kateId,
                roomId,
                atSecondHour.AddMinutes(2),
                atSecondHour.AddMinutes(2)),
            new UserEnteredRoom(
                Guid.NewGuid(),
                bobId,
                roomId,
                atSecondHour.AddMinutes(2),
                atSecondHour.AddMinutes(2)),
            new UserEnteredRoom(
                Guid.NewGuid(),
                Guid.NewGuid(),
                roomId,
                atSecondHour.AddMinutes(3),
                atSecondHour.AddMinutes(3)),
            new UserGaveHighFive(
                Guid.NewGuid(),
                bobId,
                roomId,
                Guid.NewGuid(),
                atSecondHour.AddMinutes(40),
                atSecondHour.AddMinutes(40)),
            new UserGaveHighFive(
                Guid.NewGuid(),
                bobId,
                roomId,
                Guid.NewGuid(),
                atSecondHour.AddMinutes(40),
                atSecondHour.AddMinutes(40)),
            new UserGaveHighFive(
                Guid.NewGuid(),
                bobId,
                roomId,
                Guid.NewGuid(),
                atSecondHour.AddMinutes(40),
                atSecondHour.AddMinutes(40)),
            new UserGaveHighFive(
                Guid.NewGuid(),
                kateId,
                roomId,
                Guid.NewGuid(),
                atSecondHour.AddMinutes(50),
                atSecondHour.AddMinutes(50)),
            new UserGaveHighFive(
                Guid.NewGuid(),
                kateId,
                roomId,
                Guid.NewGuid(),
                atSecondHour.AddMinutes(50),
                atSecondHour.AddMinutes(50)),
        }
        .Union(Enumerable.Range(0, 15).Select(x => new UserLeftComment(
            Guid.NewGuid(),
            bobId,
            roomId,
            $"Some Bob's comment {x}",
            atSecondHour.AddMinutes(50),
            atSecondHour.AddMinutes(50))));
        
        return eventsAtFirstHour
            .Union(eventsAtSecondHour)
            .ToArray();
    }
}