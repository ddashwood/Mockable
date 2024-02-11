using Mockable.Example.UkDates.Services;

namespace Mockable.Example.UkDates.Tests.Configurators;

internal class FakeItEasyDateServiceConfigurators
{
    public IBankHolidaysService BankHolidaysService { get; set; } = null!;
}
