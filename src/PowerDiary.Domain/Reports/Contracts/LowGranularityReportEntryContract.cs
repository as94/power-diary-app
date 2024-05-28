namespace PowerDiary.Domain.Reports.Contracts;

public class LowGranularityReportEntryContract
{
    public LowGranularityReportEntryContract(DateTime creatingHour, string[] messages)
    {
        CreatingHour = creatingHour;
        FormatedTime = $"{creatingHour:hh:mmtt}";
        Messages = messages;
    }

    public DateTime CreatingHour { get; }
    public string FormatedTime { get; }
    public string[] Messages { get; }
}