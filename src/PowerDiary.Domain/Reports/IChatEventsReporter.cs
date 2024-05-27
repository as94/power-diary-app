using PowerDiary.Domain.Reports.Contracts;

namespace PowerDiary.Domain.Reports;

public interface IChatEventsReporter
{
    Task<HighGranularityReportContract> GetHighGranularityReportAsync(DateTime currentDate, CancellationToken ct);
    Task<LowGranularityReportContract> GetLowGranularityReportAsync(DateTime currentDate, CancellationToken ct);
}