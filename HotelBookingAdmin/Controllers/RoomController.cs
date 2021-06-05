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
      ViewBag.Phongs = DBRoom.getPhongs();
      ViewBag.RatePercents = DBHelper.getRatePercent();

      return View();
    }

    [HttpGet]
    public JsonResult GetPhong()
    {
      List<Phong> phongs = DBRoom.getPhongs();
      return Json(new { listPhong = phongs }, JsonRequestBehavior.AllowGet);
    }

    [HttpPost]
    public JsonResult UpdateRoom(Phong updatePhong)
    {
      List<Phong> listPhong = DBRoom.updateRoom(updatePhong);
      HoaDon hd = DBOrder.getHoaDons().FirstOrDefault(item => item.maPhong == updatePhong.maPhong);
      DBOrder.updateHoaDon(hd);
      return Json(new { phongs = listPhong });
    }
  }
}