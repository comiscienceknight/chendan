using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ChendanKelly.Models;
using ChendanKelly.Data;

namespace ChendanKelly.Controllers
{
    public class HomeController : Controller
    {
        public IDatabaseRepository _dbRepo;

        public HomeController(IDatabaseRepository dbRepo)
        {
            _dbRepo = dbRepo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult DataSource()
        {
            return View();
        }

        public async Task<IActionResult> Results()
        {
            await _dbRepo.GetResultAsync(Convert.ToDateTime("2018-07-18"));
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
