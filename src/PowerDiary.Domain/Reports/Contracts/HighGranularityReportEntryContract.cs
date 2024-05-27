namespace PowerDiary.Domain.Reports.Contracts;

public class HighGranularityReportEntryContract
{
    public HighGranularityReportEntryContract(string formatedTime, string[] messages)
    {
        FormatedTime = formatedTime;
        Messages = messages;
    }

    public string FormatedTime { get; }
    public string[] Messages { get; }
}