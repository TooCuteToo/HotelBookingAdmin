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
      ViewBag.DichVus = DBHelper.getServices();

      return View();
    }

    [HttpPost]
    public JsonResult UpdateOrder(HoaDon hd, string[] services)
    {
      HoaDon kq = DBHelper.updateHoaDon(hd);
      DBHelper.removeServicesDetail(kq);
      DBHelper.createServicesDetail(hd, services);
      HoaDon updatedHD = DBHelper.getHoaDon(kq);

      return Json(new { result = updatedHD, listHD = DBHelper.getHoaDons(), phongTrongs = DBHelper.getPhongs().Where(item => item.tinhTrang == "empty").ToList() });
    }

    [HttpPost]
    public JsonResult DeleteOrder(int maHD)
    {
      DBHelper.deleteOrder(maHD);
      return Json(new { listHD = DBHelper.getHoaDons(), phongTrongs = DBHelper.getPhongs().Where(item => item.tinhTrang == "empty").ToList() });
    }

    [HttpPost]
    public JsonResult CreateOrder(HoaDon newHD, string[] services)
    {
      newHD.tinhTrang = false;
      newHD.maNV = ((NhanVien)Session["NhanVien"]).MaNV;

      HoaDon kq = DBHelper.createHoaDon(newHD);
      DBHelper.createServicesDetail(newHD, services);
      HoaDon hd = DBHelper.getHoaDon(kq);

      return Json(new { result = hd, listHD = DBHelper.getHoaDons(), phongTrongs = DBHelper.getPhongs().Where(item => item.tinhTrang == "empty").ToList() });
    }

    [HttpPost]
    public JsonResult GetServicesDetail(int maHD)
    {
      List<ChiTietDichVu> services = DBHelper.getServiceDetailByOrder(maHD);
      return Json(new { services });
    }
  }
}