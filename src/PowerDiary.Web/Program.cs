using PowerDiary.Data;
using PowerDiary.Data.DummyData;
using PowerDiary.Domain;
using PowerDiary.Domain.ChatEvents;
using PowerDiary.Domain.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterData();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

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
    DateTime.Now.Date.AddHours(12)), CancellationToken.None);

await chatRepository.AddRangeAsync(ChatEventsData.GetEventsForLowGranularity(
    bobId,
    kateId,
    roomId,
    DateTime.Now.Date.AddHours(17)), CancellationToken.None);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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