using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HotelBookingAdmin.Models;
using HotelBookingAdmin.Helpers;

namespace HotelBookingAdmin.Controllers
{
  public class CustomerController : Controller
  {
    #region Login/Register/Profile
    // GET: Customer
    public ActionResult Index()
    {
      return View();
    }

    [HttpPost]
    public JsonResult Index(FormCollection fc)
    {
      string email = fc["email"];
      string password = fc["password"];
      NhanVien nv = DBEmployee.getLoginEmployee(email, password);

      if (nv != null)
      {
        Session["NhanVien"] = nv;
        return Json(new { message = "success" });
      }

      return Json(new { message = "fail" });
    }

    public ActionResult Register()
    {
      return View();
    }

    [HttpPost]
    public JsonResult Register(FormCollection fc)
    {
      string fullName = fc["fullName"];
      string phoneNumber = fc["phoneNumber"];
      string email = fc["email"];
      string dob = fc["dob"];

      NhanVien nv = DBEmployee.createEmployee(fullName, phoneNumber, dob, email);

      if (nv != null && nv.TenNV != null)
      {
        return Json(new { message = "Success!!!" });
      }

      return Json(new { message = email + " is already existed" });
    }

    public ActionResult PasswordRecovery()
    {
      return View();
    }

    [HttpPost]
    public JsonResult PasswordRecovery(string email)
    {
      string recoveryPassword = DBEmployee.getPassword(email);

      if (recoveryPassword.Length != 0 || recoveryPassword != null)
      {
        return Json(new { password = recoveryPassword });
      }

      return Json(new { password = "" });

    }

    public ActionResult Logout()
    {
      Session["NhanVien"] = null;
      return RedirectToAction("Index", "Customer");
    }

    public ActionResult Profile()
    {
      return View();
    }

    #endregion

    #region Employee

    [HttpGet]
    public ActionResult Employee()
    {
      ViewBag.NhanViens = DBEmployee.getEmployees();
      return View();
    }

    [HttpPost]
    public JsonResult Employee(int maNV)
    {
      string message = DBEmployee.deleteEmployee(maNV);
      return Json(new { message });
    }

    [HttpPost]
    public JsonResult UpdateEmployee(NhanVien nv)
    {
      NhanVien result = DBEmployee.updateEmployee(nv);
      NhanVien loginNv = (NhanVien)Session["NhanVien"];

      if (nv.MaNV == loginNv.MaNV)
      {
        Session["NhanVien"] = nv;
        return Json(new { loginNv = nv });
      }

      return Json(new { nhanVien = result });
    }

    #endregion

    #region Customer

    [HttpGet]
    public ActionResult Customer()
    {
      List<KhachHang> customers = DBCustomer.getCustomers();
      ViewBag.KhachHangs = customers;
      ViewBag.Males = customers.Where(item => item.gioiTinh == "Male").ToList();
      ViewBag.Females = customers.Where(item => item.gioiTinh == "Female").ToList();

      List<HoaDon> hoaDons = DBOrder.getHoaDons();
      List<string> listEmail = new List<string>();

      float bookingCount = 0;

      foreach (HoaDon hd in hoaDons)
      {
        foreach (KhachHang kh in customers)
        {
          if (kh.email == hd.email && !listEmail.Contains(hd.email))
          {
            bookingCount++;
            listEmail.Add(kh.email);
            continue;
          }
        }
      }

      ViewBag.Booking = bookingCount;
      ViewBag.NotYet = customers.Count - bookingCount;
      return View();
    }

    [HttpPost]
    public JsonResult UpdateCustomer(KhachHang updatedKH)
    {
      KhachHang kh = DBCustomer.UpdateCustomer(updatedKH);

      List<KhachHang> customers = DBCustomer.getCustomers();
      float males = customers.Where(item => item.gioiTinh == "Male").ToList().Count;
      float females = customers.Where(item => item.gioiTinh == "Female").ToList().Count;

      List<HoaDon> hoaDons = DBOrder.getHoaDons();
      List<string> listEmail = new List<string>();

      float bookingCount = 0;

      foreach (HoaDon hd in hoaDons)
      {
        foreach (KhachHang item in customers)
        {
          if (item.email == hd.email && !listEmail.Contains(hd.email))
          {
            bookingCount++;
            listEmail.Add(item.email);
            continue;
          }
        }
      }

      float notYet = customers.Count - bookingCount;

      return Json(new { khachHang = kh,  males, females, bookingCount, notYet, customers = customers.Count });
    }

    [HttpPost]
    public JsonResult DeleteCustomer(KhachHang deletedKH)
    {
      string message = DBCustomer.deleteCustomer(deletedKH);
      List<KhachHang> customers = DBCustomer.getCustomers();
      float males = customers.Where(item => item.gioiTinh == "Male").ToList().Count;
      float females = customers.Where(item => item.gioiTinh == "Female").ToList().Count;

      List<HoaDon> hoaDons = DBOrder.getHoaDons();
      List<string> listEmail = new List<string>();

      float bookingCount = 0;

      foreach (HoaDon hd in hoaDons)
      {
        foreach (KhachHang item in customers)
        {
          if (item.email == hd.email && !listEmail.Contains(hd.email))
          {
            bookingCount++;
            listEmail.Add(item.email);
            continue;
          }
        }
      }

      float notYet = customers.Count - bookingCount;

      return Json(new { message, males, females, bookingCount, notYet, customers = customers.Count });
    }

    [HttpPost]
    public JsonResult CreateCustomer(KhachHang newCustomer)
    {
      KhachHang kh = DBCustomer.createCustomer(newCustomer);
      List<KhachHang> customers = DBCustomer.getCustomers();
      float males = customers.Where(item => item.gioiTinh == "Male").ToList().Count;
      float females = customers.Where(item => item.gioiTinh == "Female").ToList().Count;

      List<HoaDon> hoaDons = DBOrder.getHoaDons();
      List<string> listEmail = new List<string>();

      float bookingCount = 0;

      foreach (HoaDon hd in hoaDons)
      {
        foreach (KhachHang item in customers)
        {
          if (item.email == hd.email && !listEmail.Contains(hd.email))
          {
            bookingCount++;
            listEmail.Add(item.email);
            continue;
          }
        }
      }

      float notYet = customers.Count - bookingCount;

      return Json(new { kh, males, females, bookingCount, notYet, customers = customers.Count });
    }

    #endregion
  }
}