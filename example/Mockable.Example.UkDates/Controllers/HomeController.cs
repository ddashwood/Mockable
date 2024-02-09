using Microsoft.AspNetCore.Mvc;
using Mockable.Example.UkDates.Models;
using Mockable.Example.UkDates.Services;
using System.Diagnostics;

namespace Mockable.Example.UkDates.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDateService _dateService;

        public HomeController(ILogger<HomeController> logger, IDateService dateService)
        {
            _logger = logger;
            _dateService = dateService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(DateViewModel dateTimeVm)
        {
            dateTimeVm.Description = await _dateService.GetDateDescriptionAsync(dateTimeVm.Date, dateTimeVm.Month, dateTimeVm.Year);
            return View(dateTimeVm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
