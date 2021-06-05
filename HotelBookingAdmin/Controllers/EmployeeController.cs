using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HotelBookingAdmin.Helpers;
using HotelBookingAdmin.Models;

namespace HotelBookingAdmin.Controllers
{
  public class EmployeeController : Controller
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
  }
}