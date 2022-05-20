using System.Globalization;

namespace JobSearcher.CoreDomains.BaseEntity;

public class Core
{
    public Core()
    {
        CurrentTime = GetCurrentTime();
        CurrentDate = GetCurrentDate();
    }
    private PersianCalendar _persianCalendar;
    public PersianCalendar Calendar()
    {
        if (_persianCalendar == null)
            return new PersianCalendar();
        return _persianCalendar;
    }
    
    public int id { get; set; }
    public string CurrentTime { get; set; }
    public string CurrentDate { get; set; }
    public DateTimeOffset OffsetCreation { get; set; }
    public bool IsDeleted { get; set; }
    public string? OffsetModification { get; set; }
    public string GetCurrentTime() => Calendar().GetHour(DateTime.Now) + ":" + Calendar().GetMinute(DateTime.Now) +
                                      ":" + Calendar().GetSecond(DateTime.Now);
    public string GetCurrentDate() => Calendar().GetYear(DateTime.Now) + "/" + Calendar().GetMonth(DateTime.Now) + "/" +
                                      Calendar().GetDayOfMonth(DateTime.Now);
    
}