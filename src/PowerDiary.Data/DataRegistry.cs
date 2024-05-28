using Microsoft.Extensions.DependencyInjection;
using PowerDiary.Domain;
using PowerDiary.Domain.Reports;

namespace PowerDiary.Data;

public static class DataRegistry
{
    public static void RegisterData(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<IUserRepository>(new SqlUserRepository(connectionString));
        services.AddSingleton<IChatEventsRepository>(new SqlChatEventsRepository(connectionString));
        services.AddSingleton<IChatEventsReporter, ChatEventsReporter>();
    }
}