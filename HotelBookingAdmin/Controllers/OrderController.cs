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
      ViewBag.HoaDons = DBOrder.getHoaDons();
      ViewBag.KhachHangs = DBCustomer.getCustomers();
      ViewBag.NhanViens = DBEmployee.getEmployees();
      ViewBag.PhongTrongs = DBRoom.getPhongs().Where(item => item.tinhTrang == "empty").ToList();
      ViewBag.DichVus = DBServices.getServices();

      return View();
    }

    [HttpPost]
    public JsonResult UpdateOrder(HoaDon hd, string[] services)
    {
      DBServices.removeServicesDetail(hd);
      DBServices.createServicesDetail(hd, services);
      HoaDon kq = DBOrder.updateHoaDon(hd);
      HoaDon updatedHD = DBOrder.getHoaDon(kq);

      return Json(new { result = updatedHD, listHD = DBOrder.getHoaDons().Where(item => item.tinhTrang == false).ToList(), phongTrongs = DBRoom.getPhongs().Where(item => item.tinhTrang == "empty").ToList() });
    }

    [HttpPost]
    public JsonResult DeleteOrder(int maHD)
    {
      DBOrder.deleteOrder(maHD);
      return Json(new { listHD = DBOrder.getHoaDons(), phongTrongs = DBRoom.getPhongs().Where(item => item.tinhTrang == "empty").ToList() });
    }

    [HttpPost]
    public JsonResult CreateOrder(HoaDon newHD, string[] services)
    {
      newHD.tinhTrang = false;
      newHD.maNV = ((NhanVien)Session["NhanVien"]).MaNV;

      HoaDon kq = DBOrder.createHoaDon(newHD);
      DBServices.createServicesDetail(newHD, services);
      HoaDon hd = DBOrder.updateHoaDon(kq);

      return Json(new { result = hd, listHD =  DBOrder.getHoaDons().Where(item => item.tinhTrang == false).ToList(), phongTrongs = DBRoom.getPhongs().Where(item => item.tinhTrang == "empty").ToList() });
    }

    [HttpPost]
    public JsonResult GetServicesDetail(int maHD)
    {
      List<ChiTietDichVu> services = DBServices.getServiceDetailByOrder(maHD);
      return Json(new { services });
    }
  }
}