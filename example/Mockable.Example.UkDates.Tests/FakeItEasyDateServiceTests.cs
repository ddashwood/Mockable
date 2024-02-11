using FakeItEasy;
using Mockable.Example.UkDates.Models;
using Mockable.Example.UkDates.Services;
using Mockable.Example.UkDates.Tests.Configurators;
using Mockable.FakeItEasy;

namespace Mockable.Example.UkDates.Tests;

public class FakeItEasyDateServiceTests
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

    [Fact]
    public async Task NoBankHolidaysTest()
    {
        // Arrange
        var serviceFactory = new ServiceFactory();
        var service = serviceFactory.Create<DateService, FakeItEasyDateServiceConfigurators>(out var configurators);
        A.CallTo(() => configurators.BankHolidaysService.GetBankHolidaysAsync()).Returns<BankHolidayCollection?>(null);

        // Act
        var result = await service.GetDateDescriptionAsync(2024, 2, 1);

        // Assert
        Assert.Equal("1 February 2024 is a Thursday. There was a problem fetching the bank holidays.", result);
    }

    [Fact]
    public async Task BankHolidayTest()
    {
        // Arrange
        var serviceFacotry = new ServiceFactory();
        var service = serviceFacotry.Create<DateService, FakeItEasyDateServiceConfigurators>(out var configurators);

        var bankHolidays = new BankHolidayCollection
        {
            englandandwales = new Division
            {
                events = new Event[]
                {
                    new Event { date = new DateOnly(2023, 12, 25), title = "Christmas Day"}
                }
            },
            scotland = new Division {  events = new Event[0] },
            northernireland = new Division {  events = new Event[0] }
        };
        A.CallTo(() => configurators.BankHolidaysService.GetBankHolidaysAsync()).Returns(bankHolidays);

        // Act
        var result = await service.GetDateDescriptionAsync(2023, 12, 25);

        // Assert
        Assert.Equal("25 December 2023 is a Monday. In England and Wales, this is Christmas Day.", result);
    }

    [Fact]
    public async Task BankHolidayWithNoteTest()
    {
        // Arrange
        var serviceFacotry = new ServiceFactory();
        var service = serviceFacotry.Create<DateService, FakeItEasyDateServiceConfigurators>(out var configurators);

        var bankHolidays = new BankHolidayCollection
        {
            englandandwales = new Division
            {
                events = new Event[]
                {
                    new Event { date = new DateOnly(2022, 12, 27), title = "Christmas Day", notes = "Substitute"}
                }
            },
            scotland = new Division { events = new Event[0] },
            northernireland = new Division { events = new Event[0] }
        };
        A.CallTo(() => configurators.BankHolidaysService.GetBankHolidaysAsync()).Returns(bankHolidays);

        // Act
        var result = await service.GetDateDescriptionAsync(2022, 12, 27);

        // Assert
        Assert.Equal("27 December 2022 is a Tuesday. In England and Wales, this is Christmas Day (Substitute).", result);
    }
}