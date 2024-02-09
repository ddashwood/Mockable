﻿using Mockable.Example.UkDates.Models;
using System.Linq;

namespace Mockable.Example.UkDates.Services;

internal class DateService : IDateService
{
    private readonly IBankHolidaysService _bankHolidaysService;

    public DateService(IBankHolidaysService bankHolidaysService)
    {
        _bankHolidaysService = bankHolidaysService;
    }

    public async Task<string> GetDateDescriptionAsync(int day, int month, int year)
    {
        DateOnly date;

        try
        {
            date = new DateOnly(year, month, day);
        }
        catch(ArgumentOutOfRangeException)
        {
            return "The date supplied is not a valid date";
        }

        var result = $"{date.Day} {date.ToString("MMMM")} {date.Year} is a {date.DayOfWeek}. ";

        var bankHolidays = await _bankHolidaysService.GetBankHolidaysAsync();
        if( bankHolidays == null )
        {
            result += "There was a problem fetching the bank holidays.";
            return result;
        }

        result += GetBankHolidayDescription(date, bankHolidays.englandandwales, "England and Wales");
        result += GetBankHolidayDescription(date, bankHolidays.scotland, "Scotland");
        result += GetBankHolidayDescription(date, bankHolidays.northernireland, "Northern Ireland");

        return result.Trim();
    }

    private string? GetBankHolidayDescription(DateOnly date, Division divisionHolidays, string divisionName)
    {
        var match = divisionHolidays.events.FirstOrDefault(e => e.date == date);
        if (match == null)
        {
            return null;
        }

        if (string.IsNullOrWhiteSpace(match.notes))
        {
            return $"In {divisionName}, this is {match.title}. ";
        }
        else
        {
            return $"In {divisionName}, this is {match.title} ({match.notes}). ";
        }
    }
}