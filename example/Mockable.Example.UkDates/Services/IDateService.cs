namespace Mockable.Example.UkDates.Services;

public interface IDateService
{
    Task<string> GetDateDescriptionAsync(int day, int month, int year);
}
