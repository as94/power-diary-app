using PowerDiary.Domain.Models;

namespace PowerDiary.Domain;

public interface IUserRepository
{
    Task AddRangeAsync(IEnumerable<User> users, CancellationToken ct);
    Task<IEnumerable<User>> GetRangeAsync(IEnumerable<Guid> userIds, CancellationToken ct);
}