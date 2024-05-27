using PowerDiary.Domain;
using PowerDiary.Domain.Models;

namespace PowerDiary.Repositories.Fake;

public sealed class FakeUserRepository : IUserRepository
{
    private readonly List<User> _users = new();
    
    public Task AddRangeAsync(IEnumerable<User> users, CancellationToken ct)
    {
        _users.AddRange(users);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<User>> GetRangeAsync(IEnumerable<Guid> userIds, CancellationToken ct)
    {
        return Task.FromResult(_users.Where(u => userIds.Contains(u.Id)));
    }
}