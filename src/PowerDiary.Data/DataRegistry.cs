using Microsoft.Extensions.DependencyInjection;
using PowerDiary.Domain;
using PowerDiary.Domain.Reports;
using PowerDiary.Repositories.Fake;

namespace PowerDiary.Data;

public static class DataRegistry
{
    public static void RegisterData(this IServiceCollection services)
    {
        services.AddSingleton<IUserRepository, FakeUserRepository>();
        services.AddSingleton<IChatEventsRepository, FakeChatEventsRepository>();
        services.AddSingleton<IChatEventsReporter, ChatEventsReporter>();
    }
}