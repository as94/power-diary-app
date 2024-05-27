using FluentAssertions;
using PowerDiary.Domain;
using PowerDiary.Domain.Models;
using PowerDiary.Domain.Reports;
using PowerDiary.Domain.Reports.Contracts;
using PowerDiary.Repositories.Fake;
using PowerDiary.Tests.DummyData;

namespace PowerDiary.Tests;

[TestFixture]
public class ChatEventsReporterTests
{
    private IUserRepository _userRepository;
    private IChatEventsRepository _chatEventsRepository;
    private IChatEventsReporter _reporter;
    
    [SetUp]
    public void SetUp()
    {
        _userRepository = new FakeUserRepository();
        _chatEventsRepository = new FakeChatEventsRepository();
        _reporter = new ChatEventsReporter(_chatEventsRepository, _userRepository);
    }

    [Test]
    public async Task WhenHaveUsersAndChatEventsCanGetReportWithHighGranularity()
    {
        var roomId = Guid.NewGuid();
        var bobId = Guid.NewGuid();
        var kateId = Guid.NewGuid();
        var initialDateTime = DateTime.Today.Add(TimeSpan.FromHours(17));
        await _userRepository.AddRangeAsync(new []
        {
            new User(bobId, "Bob"),
            new User(kateId, "Kate")
        }, CancellationToken.None);
        var chatEvents = ChatEventsData.GetBunchOfEvents(bobId, kateId, roomId, initialDateTime);
        await _chatEventsRepository.AddRangeAsync(chatEvents, CancellationToken.None);

        var report =
            await _reporter.GetHighGranularityReportAsync(
                initialDateTime.Date,
                CancellationToken.None);

        report.Should().BeEquivalentTo(new HighGranularityReportContract(
        [
            new HighGranularityReportEntryContract("05:00PM", "Bob enters the room"),
            new HighGranularityReportEntryContract("05:05PM", "Kate enters the room"),
            new HighGranularityReportEntryContract("05:15PM", "Bob comments: \"Hey, Kate - high five?\""),
            new HighGranularityReportEntryContract("05:17PM", "Kate high-fives Bob"),
            new HighGranularityReportEntryContract("05:18PM", "Bob leaves"),
            new HighGranularityReportEntryContract("05:20PM", "Kate comments: \"Oh, typical\""),
            new HighGranularityReportEntryContract("05:21PM", "Kate leaves")
        ]));
    }

    [Test]
    public async Task WhenHaveUsersAndChatEventsCanGetReportWithLowGranularity()
    {
        var roomId = Guid.NewGuid();
        var bobId = Guid.NewGuid();
        var kateId = Guid.NewGuid();
        var initialDateTime = DateTime.Today.Add(TimeSpan.FromHours(17));
        await _userRepository.AddRangeAsync(new []
        {
            new User(bobId, "Bob"),
            new User(kateId, "Kate")
        }, CancellationToken.None);
        var chatEvents = ChatEventsData.GetBunchOfEvents(bobId, kateId, roomId, initialDateTime);
        await _chatEventsRepository.AddRangeAsync(chatEvents, CancellationToken.None);

        var report =
            await _reporter.GetLowGranularityReportAsync(
                initialDateTime.Date,
                CancellationToken.None);

        report.Should().BeEquivalentTo(new LowGranularityReportContract(
        [
            new LowGranularityReportEntryContract("05:00PM",
            [
                "2 person entered",
                "2 comments",
                "1 person high-fived .. other person",
                "2 left"
            ])
        ]));
    }
}