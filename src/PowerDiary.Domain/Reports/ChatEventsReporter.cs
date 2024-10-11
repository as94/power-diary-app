using PowerDiary.Domain.ChatEvents;
using PowerDiary.Domain.Reports.Contracts;

namespace PowerDiary.Domain.Reports;

public sealed class ChatEventsReporter : IChatEventsReporter
{
    private readonly IChatEventsRepository _repository;
    private readonly IUserRepository _userRepository;

    public ChatEventsReporter(IChatEventsRepository repository, IUserRepository userRepository)
    {
        _repository = repository;
        _userRepository = userRepository;
    }
    
    public async Task<LowGranularityReportContract> GetLowGranularityReportAsync(DateTime currentDate, CancellationToken ct)
    {
        var events = (await _repository.GetEventsAsync(
                currentDate.Date,
                currentDate.Date.AddDays(1),
                ct))
            .OrderBy(x => x.CreatedAt)
            .ToArray();

        var rows = events
            .GroupBy(e => e.CreatedAt.Hour)
            .Select(groupByHour =>
            {
                var eventByHour = groupByHour.First();
                var hourDateTime = new DateTime(
                    eventByHour.CreatedAt.Year,
                    eventByHour.CreatedAt.Month,
                    eventByHour.CreatedAt.Day,
                    eventByHour.CreatedAt.Hour, 
                    0, 
                    0);

                var messages = groupByHour
                    .GroupBy(e => e.GetType().FullName)
                    .SelectMany(groupByEventType =>
                    {
                        var eventByType = groupByEventType.First();
                        return eventByType.GetAggregatedReportStrings(groupByEventType);
                    });

                return new LowGranularityReportEntryContract(hourDateTime, messages.ToArray());
            })
            .OrderBy(e => e.CreatingHour)
            .ToArray();

        return new LowGranularityReportContract(rows);
    }

    public async Task<HighGranularityReportContract> GetHighGranularityReportAsync(DateTime currentDate, CancellationToken ct)
    {
        var events = (await _repository.GetEventsAsync(
                currentDate.Date,
                currentDate.Date.AddDays(1),
                ct))
            .OrderBy(x => x.CreatedAt)
            .ToArray();

        var userIds = events.Select(e => e.UserId).Distinct();
        var userNamesById = (await _userRepository.GetRangeAsync(userIds, ct))
            .ToDictionary(x => x.Id, y => y.Name);

        var rows = events
            .Select(e =>
            {
                var message = e.GetHighGranularityReportString(userNamesById);
                
                return new HighGranularityReportEntryContract(
                    e.CreatedAt,
                    message);
            })
            .OrderBy(e => e.CreatedAt)
            .ToArray();

        return new HighGranularityReportContract(rows);
    }
}