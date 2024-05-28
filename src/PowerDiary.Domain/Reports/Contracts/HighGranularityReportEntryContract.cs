namespace PowerDiary.Domain.Reports.Contracts;

public sealed class HighGranularityReportEntryContract
{
    public HighGranularityReportEntryContract(DateTime createdAt, string message)
    {
        CreatedAt = createdAt;
        FormatedTime = $"{createdAt:hh:mmtt}";
        Message = message;
    }

    public DateTime CreatedAt { get; }
    public string FormatedTime { get; }
    public string Message { get; }
}