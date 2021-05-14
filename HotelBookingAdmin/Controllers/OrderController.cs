using HotelBookingAdmin.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HotelBookingAdmin.Models;

namespace HotelBookingAdmin.Controllers
{
  public class OrderController : Controller
  {
    // GET: Order
    public ActionResult Index()
    {
      ViewBag.HoaDons = DBHelper.getHoaDons();
      ViewBag.KhachHangs = DBHelper.getKhachHangs();
      return View();
    }

    [HttpPost]
    public JsonResult UpdateOrder(HoaDon hd)
    {
      HoaDon updatedHD = DBHelper.updateHoaDon(hd);

      return Json(new { result = updatedHD, listHD = DBHelper.getHoaDons() });
    }

    [HttpPost]
    public JsonResult DeleteOrder(int maHD)
    {
      DBHelper.deleteOrder(maHD);
      return Json(new { listHD = DBHelper.getHoaDons() });
    }
  }
}