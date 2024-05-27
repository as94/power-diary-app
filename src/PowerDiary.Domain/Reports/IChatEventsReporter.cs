using PowerDiary.Domain.Reports.Contracts;

namespace PowerDiary.Domain.Reports;

public interface IChatEventsReporter
{
    Task<LowGranularityReportContract> GetLowGranularityReportAsync(DateTime currentDate, CancellationToken ct);
    Task<HighGranularityReportContract> GetHighGranularityReportAsync(DateTime currentDate, CancellationToken ct);
}