using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PowerDiary.Domain.Reports;
using PowerDiary.Web.Models;

namespace PowerDiary.Web.Controllers;

public class HomeController : Controller
{
    private readonly IChatEventsReporter _chatEventsReporter;

    public HomeController(IChatEventsReporter chatEventsReporter)
    {
        _chatEventsReporter = chatEventsReporter;
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var report = await _chatEventsReporter
            .GetHighGranularityReportAsync(DateTime.Now, ct);
        
        return View(report);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
            { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}