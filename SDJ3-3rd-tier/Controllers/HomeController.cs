using SDJ3_3rd_tier.DAL;
using SDJ3_3rd_tier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SDJ3_3rd_tier.Controllers
{
    public class HomeController : Controller
    {
        private FactoryContext db = new FactoryContext();
        private const string NO_CAR = "no_car";

        public ActionResult Index()
        {
            ViewBag.Title = "Dismantling facility";

            ViewData.Add("carCount", db.Cars.Count());
            ViewData.Add("packageCount", db.Packages.Count());
            ViewData.Add("palletCount", db.Pallets.Count());
            ViewData.Add("partsCount", db.Parts.Count());

            if (!String.IsNullOrEmpty(Request["status"]))
            {
                ViewData.Add(NO_CAR, true);

                return View();
            }


            ViewData.Add(NO_CAR, false);

            return View();
        }

        // GET: Home/CarSearch
        public ActionResult CarSearch()
        {
            string searchString = Request["SearchString"];

            var cars = from c in db.Cars
                       select c;

            if (!String.IsNullOrEmpty(searchString))
            {
                cars = cars.Where(s => s.VIN.Contains(searchString));
            }

            if (!cars.Any())
            {
                return RedirectToAction("Index", "Home", new { status = NO_CAR });
            }

            if (cars.Count() == 1)
            {
                ViewBag.Title = "Car Overview";

                return View("CarOverview", cars.First());
            }

            ViewBag.Title = "Cars List";

            return View(cars);

        }

        // GET: Home/CarSearch/5
        public ActionResult CarOverview(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home", new { status = NO_CAR });
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return RedirectToAction("Index", "Home", new { status = NO_CAR });
            }

            return View(car);
        }


    }
}
