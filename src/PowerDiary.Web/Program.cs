using PowerDiary.Data;
using PowerDiary.Data.DummyData;
using PowerDiary.Domain;
using PowerDiary.Domain.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("PowerDiary");
if (connectionString == null)
{
    throw new InvalidOperationException("Connection string must be");
}

builder.Services.RegisterData(connectionString);
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (args.Length > 0 && args[0] == "--update-database")
{
    var migrator = new Migrator(connectionString);
    await migrator.RunAsync(CancellationToken.None);
    
    var userRepository = app.Services.GetRequiredService<IUserRepository>();
    var chatRepository = app.Services.GetRequiredService<IChatEventsRepository>();
    var events = await chatRepository.GetEventsAsync(
        DateTime.Now.Date,
        DateTime.Now.AddDays(1),
        CancellationToken.None);

    if (!events.Any())
    {
        var bobId = Guid.NewGuid();
        var kateId = Guid.NewGuid();
        await userRepository.AddRangeAsync(new[]
        {
            new User(bobId, "Bob"),
            new User(kateId, "Kate")
        }, CancellationToken.None);

        var roomId = Guid.NewGuid();
        await chatRepository.AddRangeAsync(ChatEventsData.GetEventsForHighGranularity(
            bobId,
            kateId,
            roomId,
            DateTime.Now.Date.AddHours(11)), CancellationToken.None);

        await chatRepository.AddRangeAsync(ChatEventsData.GetEventsForLowGranularity(
            bobId,
            kateId,
            roomId,
            DateTime.Now.Date.AddHours(17)), CancellationToken.None);
    }

    Console.WriteLine("Migrations completed");
    return;
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();