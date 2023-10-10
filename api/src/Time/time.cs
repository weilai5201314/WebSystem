namespace server.Time;

public static class TimeHelper
{
    //获取北京时间
    public static DateTime BeijingTime
    {
        get
        {
            TimeZoneInfo cstTimeZone = TimeZoneInfo.FindSystemTimeZoneById("China Standard Time");
            DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, cstTimeZone);
            return cstTime;
        }
    }
}