namespace OrderService.Infrastructure;

internal static class SqlExtensions
{
    public static string ToSqlField(this string sqlField) => $"\"{sqlField}\"";
}