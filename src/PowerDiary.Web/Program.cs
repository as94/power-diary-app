using PowerDiary.Data;
using PowerDiary.Data.DummyData;
using PowerDiary.Domain;
using PowerDiary.Domain.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterData();
builder.Services.AddControllersWithViews();

var app = builder.Build();

var connectionString = app.Configuration.GetConnectionString("PowerDiary");
if (args[0] == "--update-database" && connectionString != null)
{
    var migrator = new Migrator(connectionString);
    await migrator.RunAsync(CancellationToken.None);
    return;
}

var bobId = Guid.NewGuid();
var kateId = Guid.NewGuid();
var userRepository = app.Services.GetRequiredService<IUserRepository>();
await userRepository.AddRangeAsync(new[]
{
    new User(bobId, "Bob"),
    new User(kateId, "Kate")
}, CancellationToken.None);

var chatRepository = app.Services.GetRequiredService<IChatEventsRepository>();
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