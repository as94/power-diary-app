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
        var initialDateTime = DateTime.Today.AddHours(17);
        await _userRepository.AddRangeAsync(new []
        {
            new User(bobId, "Bob"),
            new User(kateId, "Kate")
        }, CancellationToken.None);
        var chatEvents = ChatEventsData.GetEventsForHighGranularity(bobId, kateId, roomId, initialDateTime);
        await _chatEventsRepository.AddRangeAsync(chatEvents, CancellationToken.None);

        var report =
            await _reporter.GetHighGranularityReportAsync(
                initialDateTime.Date,
                CancellationToken.None);

        report.Should().BeEquivalentTo(new HighGranularityReportContract(
        [
            new HighGranularityReportEntryContract(DateTime.Today.AddHours(17), "Bob enters the room"),
            new HighGranularityReportEntryContract(DateTime.Today.AddHours(17).AddMinutes(5), "Kate enters the room"),
            new HighGranularityReportEntryContract(DateTime.Today.AddHours(17).AddMinutes(15), "Bob comments: \"Hey, Kate - high five?\""),
            new HighGranularityReportEntryContract(DateTime.Today.AddHours(17).AddMinutes(17), "Kate high-fives Bob"),
            new HighGranularityReportEntryContract(DateTime.Today.AddHours(17).AddMinutes(18), "Bob leaves"),
            new HighGranularityReportEntryContract(DateTime.Today.AddHours(17).AddMinutes(20), "Kate comments: \"Oh, typical\""),
            new HighGranularityReportEntryContract(DateTime.Today.AddHours(17).AddMinutes(21), "Kate leaves")
        ]));
    }

    [Test]
    public async Task WhenHaveChatEventsCanGetReportWithLowGranularity()
    {
        var roomId = Guid.NewGuid();
        var initialDateTime = DateTime.Today.AddHours(17);
        var chatEvents = ChatEventsData.GetEventsForLowGranularity(roomId, initialDateTime);
        await _chatEventsRepository.AddRangeAsync(chatEvents, CancellationToken.None);

        var report =
            await _reporter.GetLowGranularityReportAsync(
                initialDateTime.Date,
                CancellationToken.None);

        report.Should().BeEquivalentTo(new LowGranularityReportContract(
        [
            new LowGranularityReportEntryContract(DateTime.Today.AddHours(17),
            [
                "1 person entered",
                "2 left",
                "1 person high-fived 1 other person",
                "2 comments",
            ]),
            new LowGranularityReportEntryContract(DateTime.Today.AddHours(18),
            [
                "3 people entered",
                "1 person high-fived 3 other people",
                "1 person high-fived 2 other people",
                "15 comments"
            ])
        ]));
    }
}