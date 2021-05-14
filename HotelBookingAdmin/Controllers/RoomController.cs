using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HotelBookingAdmin.Helpers;
using HotelBookingAdmin.Models;

namespace HotelBookingAdmin.Controllers
{
  public class RoomController : Controller
  {
    // GET: Room
    public ActionResult Index()
    {
      ViewBag.StatisticsInfo = DBHelper.getStatistics();
      ViewBag.TotalRoom = DBHelper.countTotalRoom();
      ViewBag.Phongs = DBHelper.getPhongs();
      ViewBag.RatePercents = DBHelper.getRatePercent();

      return View();
    }

    [HttpGet]
    public JsonResult GetPhong()
    {
      List<Phong> phongs = DBHelper.getPhongs();
      return Json(new { listPhong = phongs }, JsonRequestBehavior.AllowGet);
    }

    [HttpPost]
    public JsonResult UpdateRoom(Phong updatePhong)
    {
      List<Phong> listPhong = DBHelper.updateRoom(updatePhong);
      return Json(new { phongs = listPhong });
    }
  }
}