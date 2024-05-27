using FluentAssertions;
using PowerDiary.Domain;
using PowerDiary.Domain.ChatEvents;
using PowerDiary.Domain.Models;
using PowerDiary.Repositories.Fake;

namespace PowerDiary.Tests;

[TestFixture]
public class ChatEventsRepositoryTests
{
    private readonly IChatEventsRepository _repository = new FakeChatEventsRepository();
    
    [Test]
    public async Task WhenAddChatEventsToRepositoryCanGetEventsFilteredByDate()
    {
        var roomId = Guid.NewGuid();
        var bobId = Guid.NewGuid();
        var at5pm = DateTime.Today.Add(TimeSpan.FromHours(17));
        var bobEntered = new UserEnteredRoom(
            Guid.NewGuid(),
            bobId,
            roomId,
            at5pm,
            at5pm);
        var kateId = Guid.NewGuid();
        var at5_05pm = at5pm.AddMinutes(5);
        var kateEntered = new UserEnteredRoom(
            Guid.NewGuid(),
            kateId,
            roomId,
            at5_05pm,
            at5_05pm);
        var at5_15 = at5pm.AddMinutes(15);
        var bobLeftComment = new UserLeftComment(
            Guid.NewGuid(),
            bobId,
            roomId,
            "Hey, Kate - high five?",
            at5_15,
            at5_15);
        var at5_17 = at5pm.AddMinutes(17);
        var kateHighFiveBob = new UserGaveHighFive(
            Guid.NewGuid(),
            kateId,
            roomId,
            bobId,
            at5_17,
            at5_17);
        var at5_18 = at5pm.AddMinutes(18);
        var bobLeftRoom = new UserLeftRoom(
            Guid.NewGuid(),
            bobId,
            roomId,
            at5_18,
            at5_18);
        var at5_20 = at5pm.AddMinutes(20);
        var kateLeftComment = new UserLeftComment(
            Guid.NewGuid(),
            kateId,
            roomId,
            "Oh, typical",
            at5_20,
            at5_20);
        var at5_21 = at5pm.AddMinutes(21);
        var kateLeftRoom = new UserLeftRoom(
            Guid.NewGuid(),
            kateId,
            roomId,
            at5_21,
            at5_21);
        await _repository.AddRangeAsync(
            new IChatEvent[]
            {
                bobEntered,
                kateEntered,
                bobLeftComment,
                kateHighFiveBob,
                bobLeftRoom,
                kateLeftComment,
                kateLeftRoom
            }, CancellationToken.None);
        
        var filteredEvents = await _repository.GetEventsAsync(
            at5pm,
            at5pm.AddHours(1),
            CancellationToken.None);

        filteredEvents.Should().BeEquivalentTo(
            new IChatEvent[]
            {
                bobEntered,
                kateEntered,
                bobLeftComment,
                kateHighFiveBob,
                bobLeftRoom,
                kateLeftComment,
                kateLeftRoom
            });
    }
}