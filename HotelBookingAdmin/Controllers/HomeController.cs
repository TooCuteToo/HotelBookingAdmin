using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HotelBookingAdmin.Helpers;
using HotelBookingAdmin.Models;

namespace HotelBookingAdmin.Controllers
{
  public class HomeController : Controller
  {
    // GET: Home
    public ActionResult Index()
    {
      NhanVien nv = (NhanVien) Session["NhanVien"];

      if (nv == null)
      {
        return RedirectToAction("Index", "Customer");
      }

      ViewBag.StatisticsInfo = DBHelper.getStatistics();
      ViewBag.TotalRoom = DBHelper.countTotalRoom();
      ViewBag.Phongs = DBRoom.getPhongs();
      ViewBag.RatePercents = DBHelper.getRatePercent();

      return View();
    }

    public ActionResult About()
    {
      ViewData["Message"] = "Your application description page.";

      return View();
    }

    public ActionResult Contact()
    {
      ViewData["Message"] = "Your contact page.";

      return View();
    }

    public ActionResult Privacy()
    {
      return View();
    }
  }
}