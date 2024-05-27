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
    
    public Task<HighGranularityReportContract> GetHighGranularityReportAsync(DateTime currentDate, CancellationToken ct)
    {
        throw new NotImplementedException();
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
                var message = e.ToString(userNamesById);
                
                return new LowGranularityReportEntryContract(
                    $"{e.CreatedAt:hh:mmtt}",
                    message);
            })
            .ToArray();

        return new LowGranularityReportContract(rows);
    }
}