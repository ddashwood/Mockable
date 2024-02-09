using Mockable.Example.UkDates.Models;
using Mockable.Example.UkDates.Services;
using Mockable.Example.UkDates.Tests.Configurators;
using Mockable.Moq;
using Moq;

namespace Mockable.Example.UkDates.Tests;

public class DateServiceTests
{
    [Fact]
    public async Task InvalidDateTest()
    {
        // Arrange
        var serviceFactory = new ServiceFactory();
        var service = serviceFactory.Create<DateService>();

        // Act
        var result = await service.GetDateDescriptionAsync(30, 2, 2024);

        // Assert
        Assert.Equal("The date supplied is not a valid date", result);
    }

    [Fact]
    public async Task NoBankHolidaysTest()
    {
        // Arrange
        var serviceFactory = new ServiceFactory();
        var service = serviceFactory.Create<DateService>();

        // Act
        var result = await service.GetDateDescriptionAsync(1, 2, 2024);

        // Assert
        Assert.Equal("1 February 2024 is a Thursday. There was a problem fetching the bank holidays.", result);
    }

    [Fact]
    public async Task BankHolidayTest()
    {
        // Arrange
        var serviceFacotry = new ServiceFactory();
        var service = serviceFacotry.Create<DateService, DateServiceConfigurators>(out var configurators);

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
        configurators.BankHolidaysService.Setup(m => m.GetBankHolidaysAsync()).ReturnsAsync(bankHolidays);

        // Act
        var result = await service.GetDateDescriptionAsync(25, 12, 2023);

        // Assert
        Assert.Equal("25 December 2023 is a Monday. In England and Wales, this is Christmas Day.", result);
    }

    [Fact]
    public async Task BankHolidayWithNoteTest()
    {
        // Arrange
        var serviceFacotry = new ServiceFactory();
        var service = serviceFacotry.Create<DateService, DateServiceConfigurators>(out var configurators);

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
        configurators.BankHolidaysService.Setup(m => m.GetBankHolidaysAsync()).ReturnsAsync(bankHolidays);

        // Act
        var result = await service.GetDateDescriptionAsync(27, 12, 2022);

        // Assert
        Assert.Equal("27 December 2022 is a Tuesday. In England and Wales, this is Christmas Day (Substitute).", result);
    }
}