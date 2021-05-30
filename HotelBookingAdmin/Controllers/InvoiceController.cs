using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HotelBookingAdmin.Helpers;
using HotelBookingAdmin.Models;

namespace HotelBookingAdmin.Controllers
{
  public class InvoiceController : Controller
  {
    // GET: Invoice
    [HttpGet]
    public ActionResult Index(string maHD)
    {
      HoaDon hd = DBOrder.getHoaDon(int.Parse(maHD));
      Phong phong = DBRoom.getPhong(hd.tenPhong);

      ViewBag.HoaDon = hd;
      ViewBag.KhachHang = DBCustomer.getCustomers().FirstOrDefault(item => item.email == hd.email);
      ViewBag.LoaiPhong = DBRoom.getLoaiPhong(hd.tenPhong);
      ViewBag.ServicesTotal = DBServices.getTotalServicesMoney(hd);
      ViewBag.Phong = phong;


      return View();
    }
  }
}