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
      ViewBag.KhachHangs = DBHelper.getCustomers();
      ViewBag.NhanViens = DBHelper.getEmployees();
      ViewBag.PhongTrongs = DBHelper.getPhongs().Where(item => item.tinhTrang == "empty").ToList();
      return View();
    }

    [HttpPost]
    public JsonResult UpdateOrder(HoaDon hd)
    {
      HoaDon updatedHD = DBHelper.updateHoaDon(hd);

      return Json(new { result = updatedHD, listHD = DBHelper.getHoaDons(), phongTrongs = DBHelper.getPhongs().Where(item => item.tinhTrang == "empty").ToList() });
    }

    [HttpPost]
    public JsonResult DeleteOrder(int maHD)
    {
      DBHelper.deleteOrder(maHD);
      return Json(new { listHD = DBHelper.getHoaDons(), phongTrongs = DBHelper.getPhongs().Where(item => item.tinhTrang == "empty").ToList() });
    }

    [HttpPost]
    public JsonResult CreateOrder(HoaDon newHD)
    {
      newHD.tinhTrang = false;
      newHD.maNV = ((NhanVien)Session["NhanVien"]).MaNV;

      HoaDon hd = DBHelper.createHoaDon(newHD);
      return Json(new { result = hd, listHD = DBHelper.getHoaDons(), phongTrongs = DBHelper.getPhongs().Where(item => item.tinhTrang == "empty").ToList() });
    }
  }
}