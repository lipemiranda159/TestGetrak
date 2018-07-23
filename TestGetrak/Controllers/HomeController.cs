using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestGetrak.Domain.Contract.Interfaces;
using TestGetrak.Models;

namespace TestGetrak.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStarShipInformationProvider _starShipInformationProvider;
        public HomeController(IStarShipInformationProvider starShipInformationProvider)
        {
            _starShipInformationProvider = starShipInformationProvider;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Página de contato";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> InformationStarShips()
        {
            var starShips = await _starShipInformationProvider.GetStarShipsInformation();
            return View(starShips);
        }
        [HttpPost]
        public async Task<IActionResult> InformationStarShipsWithDistance(int distance)
        {
            ViewBag.distance = distance;
            var starShips = await _starShipInformationProvider.GetStarShipsInformation(distance);
            return View(starShips);
        }

    }
}
