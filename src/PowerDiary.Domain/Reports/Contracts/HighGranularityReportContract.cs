namespace PowerDiary.Domain.Reports.Contracts;

public sealed class HighGranularityReportContract
{
    public HighGranularityReportContract(HighGranularityReportEntryContract[] rows)
    {
        Rows = rows;
    }

    public HighGranularityReportEntryContract[] Rows { get; }
}