namespace PowerDiary.Domain.Reports.Contracts;

public sealed class LowGranularityReportContract
{
    public LowGranularityReportContract(LowGranularityReportEntryContract[] rows)
    {
        Rows = rows;
    }

    public LowGranularityReportEntryContract[] Rows { get; }
}