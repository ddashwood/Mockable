using System.Globalization;

namespace Mockable.Example.UkDates.Models;

public class DateViewModel
{
    public int Date { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public string Description { get; set; } = "";
    public DateTime? ToDateTime()
    {
        CultureInfo enUk = new CultureInfo("en-GB");
        var valid = DateTime.TryParseExact($"{Year:0000}-{Month:00}-{Date:00}", "yyyy-MM-dd", enUk, DateTimeStyles.None, out var dateTime);

        return valid ? dateTime : null;
    }
}
