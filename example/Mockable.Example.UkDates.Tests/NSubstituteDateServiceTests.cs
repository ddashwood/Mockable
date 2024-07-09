using Mockable.Example.UkDates.Services;
using Mockable.NSubstitute;

namespace Mockable.Example.UkDates.Tests;

public class NSubstituteDateServiceTests
{
    [Fact]
    public async Task InvalidDateTest()
    {
        // Arrange
        var serviceFactory = new ServiceFactory();
        var service = serviceFactory.Create<DateService>();

        // Act
        var result = await service.GetDateDescriptionAsync(2024, 2, 30);

        // Assert
        Assert.Equal("The date supplied is not a valid date", result);
    }
}
