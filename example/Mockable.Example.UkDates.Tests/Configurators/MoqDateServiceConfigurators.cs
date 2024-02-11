using Mockable.Example.UkDates.Services;
using Moq;

namespace Mockable.Example.UkDates.Tests.Configurators;

internal class MoqDateServiceConfigurators
{
    public Mock<IBankHolidaysService> BankHolidaysService { get; set; } = null!;
}
