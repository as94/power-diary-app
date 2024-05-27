using FluentAssertions;
using PowerDiary.Domain;
using PowerDiary.Domain.ChatEvents;
using PowerDiary.Domain.Models;
using PowerDiary.Repositories.Fake;
using PowerDiary.Tests.DummyData;

namespace PowerDiary.Tests;

[TestFixture]
public class ChatEventsRepositoryTests
{
    private IChatEventsRepository _repository;

    [SetUp]
    public void SetUp()
    {
        _repository = new FakeChatEventsRepository();
    }
    
    [Test]
    public async Task WhenAddChatEventsToRepositoryCanGetEventsFilteredByDate()
    {
        var roomId = Guid.NewGuid();
        var bobId = Guid.NewGuid();
        var kateId = Guid.NewGuid();
        var initialDateTime = DateTime.Today.Add(TimeSpan.FromHours(17));
        var chatEvents = ChatEventsData.GetBunchOfEvents(bobId, kateId, roomId, initialDateTime);
        await _repository.AddRangeAsync(chatEvents, CancellationToken.None);
        
        var filteredEvents = await _repository.GetEventsAsync(
            initialDateTime,
            initialDateTime.AddHours(1),
            CancellationToken.None);

        filteredEvents.Should().BeEquivalentTo(chatEvents);
    }
}