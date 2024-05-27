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
                var formattedTime = $"{hourDateTime:hh:mmtt}";

                var messages = groupByHour
                    .GroupBy(e => e.GetType().FullName)
                    .SelectMany(groupByEventType =>
                    {
                        var eventByType = groupByEventType.First();
                        
                        if (groupByEventType.Key == typeof(UserGaveHighFive).FullName)
                        {
                            var highFiveMessagesByUserId = groupByEventType
                                .GroupBy(e => e.UserId);

                            var highFiveMessages = highFiveMessagesByUserId
                                .Select(e =>
                                {
                                    var highFiveRecipientsCount = e.Count();
                                    return string.Format(
                                        eventByType.GetLowGranularityReportFormat(highFiveRecipientsCount),
                                        highFiveRecipientsCount);
                                });
                            
                            return highFiveMessages;
                        }
                        
                        var count = groupByEventType.Count();

                        return [string.Format(eventByType.GetLowGranularityReportFormat(count), count)];
                    });

                return new LowGranularityReportEntryContract(formattedTime, messages.ToArray());
            })
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
                    $"{e.CreatedAt:hh:mmtt}",
                    message);
            })
            .ToArray();

        return new HighGranularityReportContract(rows);
    }
}