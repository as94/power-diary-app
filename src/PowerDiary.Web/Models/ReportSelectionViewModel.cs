using PowerDiary.Domain.Reports.Contracts;

namespace PowerDiary.Web.Models;

public class ReportSelectionViewModel
{
    public DateTime SelectedDate { get; set; }
    public string SelectedOption { get; set; }
    
    public HighGranularityReportContract? HighGranularityReport { get; set; }
    public LowGranularityReportContract? LowGranularityReport { get; set; }
}