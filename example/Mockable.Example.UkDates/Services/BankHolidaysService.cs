using Mockable.Example.UkDates.Models;
using System.Text.Json;

namespace Mockable.Example.UkDates.Services;

internal class BankHolidaysService : IBankHolidaysService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public BankHolidaysService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task<BankHolidayCollection?> GetBankHolidaysAsync()
    {
        var httpClient = _httpClientFactory.CreateClient();

        var response = await httpClient.GetAsync(_configuration["BankHolidaysURL"]);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<BankHolidayCollection>(json);
        return result;
    }
}
