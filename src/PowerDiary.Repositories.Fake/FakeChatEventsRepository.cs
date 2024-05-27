using PowerDiary.Domain;
using PowerDiary.Domain.Models;

namespace PowerDiary.Repositories.Fake;

public sealed class FakeChatEventsRepository : IChatEventsRepository
{
    private readonly List<IChatEvent> _chatEvents = new();
    
    public Task AddRangeAsync(IEnumerable<IChatEvent> events, CancellationToken ct)
    {
        _chatEvents.AddRange(events);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<IChatEvent>> GetEventsAsync(DateTime from, DateTime to, CancellationToken ct)
    {
        var filteredChatEvents = _chatEvents
            .Where(x => x.CreatedAt >= from && x.CreatedAt <= to);
        
        return Task.FromResult(filteredChatEvents);
    }
}