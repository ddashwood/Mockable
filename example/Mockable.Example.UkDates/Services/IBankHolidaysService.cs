using Mockable.Example.UkDates.Models;

namespace Mockable.Example.UkDates.Services;

public interface IBankHolidaysService
{
    Task<BankHolidayCollection?> GetBankHolidaysAsync();
}
