using FluentAssertions;
using PowerDiary.Domain;
using PowerDiary.Domain.Models;
using PowerDiary.Repositories.Fake;

namespace PowerDiary.Tests;

[TestFixture]
public class UserRepositoryTests
{
    private IUserRepository _repository;
    
    [SetUp]
    public void SetUp()
    {
        _repository = new FakeUserRepository();
    }
    
    [Test]
    public async Task WhenAddSeveralUsersToRepositoryCanGetTheir()
    {
        var userIds = new[] { Guid.NewGuid(), Guid.NewGuid() };
        var users = userIds
            .Select(id => new User(id, "User " + id))
            .ToArray();
        await _repository.AddRangeAsync(users, CancellationToken.None);

        var existingUsers = await _repository.GetRangeAsync(
            userIds,
            CancellationToken.None);

        existingUsers.Should().BeEquivalentTo(users);
    }
}