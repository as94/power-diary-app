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
    public IActionResult Index()
    {
        var reportSelectionViewModel = new ReportSelectionViewModel
        {
            SelectedDate = DateTime.Now.Date
        };
        
        return View(reportSelectionViewModel);
    }
    
    [HttpPost]
    public async Task<IActionResult> GenerateReport(
        ReportSelectionViewModel model,
        CancellationToken ct)
    {
        if (model.SelectedOption == "High")
        {
            var report = await _chatEventsReporter
                .GetHighGranularityReportAsync(model.SelectedDate, ct);

            model.HighGranularityReport = report;
        
            return View("Report", model);
        }
        
        if (model.SelectedOption == "Low")
        {
            var report = await _chatEventsReporter
                .GetLowGranularityReportAsync(model.SelectedDate, ct);
            
            model.LowGranularityReport = report;
        
            return View("Report", model);
        }

        throw new NotSupportedException();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
            { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}