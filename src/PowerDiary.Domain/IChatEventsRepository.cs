using PowerDiary.Domain.ChatEvents;
using PowerDiary.Domain.Models;

namespace PowerDiary.Domain;

public interface IChatEventsRepository
{
    public Task AddRangeAsync(IEnumerable<IChatEvent> events, CancellationToken ct);
    public Task<IEnumerable<IChatEvent>> GetEventsAsync(DateTime from, DateTime to, CancellationToken ct);
}