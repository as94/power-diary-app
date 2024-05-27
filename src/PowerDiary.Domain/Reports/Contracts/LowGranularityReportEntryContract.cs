namespace PowerDiary.Domain.Reports.Contracts;

public class LowGranularityReportEntryContract
{
    public LowGranularityReportEntryContract(string formatedTime, string[] messages)
    {
        FormatedTime = formatedTime;
        Messages = messages;
    }

    public string FormatedTime { get; }
    public string[] Messages { get; }
}