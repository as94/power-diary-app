namespace PowerDiary.Domain.Reports.Contracts;

public sealed class HighGranularityReportEntryContract
{
    public HighGranularityReportEntryContract(string formatedTime, string message)
    {
        FormatedTime = formatedTime;
        Message = message;
    }

    public string FormatedTime { get; }
    public string Message { get; }
}