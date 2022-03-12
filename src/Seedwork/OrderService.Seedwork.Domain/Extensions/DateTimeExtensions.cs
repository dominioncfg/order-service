namespace OrderService.Seedwork.Domain;

public static class DateTimeExtensions
{
    public static DateTime SetKindUtc(this DateTime dateTime)
    {
        if (dateTime.Kind == DateTimeKind.Utc)  
            return dateTime;
        return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
    }
}
