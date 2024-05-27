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
    
    public async Task<HighGranularityReportContract> GetHighGranularityReportAsync(DateTime currentDate, CancellationToken ct)
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
                var formattedTime = $"{eventByHour.CreatedAt:hh:mmtt}";

                var messages = groupByHour
                    .GroupBy(e => e.GetType().FullName)
                    .Select(groupByEventType =>
                    {
                        var eventByType = groupByEventType.First();
                        var count = groupByEventType.Count();

                        return string.Format(eventByType.GetHighGranularityReportFormat, count);
                    });

                return new HighGranularityReportEntryContract(formattedTime, messages.ToArray());
            })
            .ToArray();

        return new HighGranularityReportContract(rows);
    }

    public async Task<LowGranularityReportContract> GetLowGranularityReportAsync(DateTime currentDate, CancellationToken ct)
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
                var message = e.GetLowGranularityReportString(userNamesById);
                
                return new LowGranularityReportEntryContract(
                    $"{e.CreatedAt:hh:mmtt}",
                    message);
            })
            .ToArray();

        return new LowGranularityReportContract(rows);
    }
}