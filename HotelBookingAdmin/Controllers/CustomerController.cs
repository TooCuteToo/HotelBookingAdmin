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
    // GET: Customer
    public ActionResult Index()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Index(FormCollection fc)
    {
      string email = fc["email"];
      string password = fc["password"];
      NhanVien nv = DBHelper.getLoginEmployee(email, password);

      if (nv != null)
      {
        Session["NhanVien"] = nv;
        return RedirectToAction("Index", "Home");
      }

      return RedirectToAction("Index", "Customer");
    }

    public ActionResult Register()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Register(FormCollection fc)
    {
      string fullName = fc["fullName"];
      string phoneNumber = fc["phoneNumber"];
      string email = fc["email"];
      string dob = fc["dob"];

      NhanVien nv = DBHelper.createEmployee(fullName, phoneNumber, dob, email);

      if (nv != null)
      {
        return RedirectToAction("Index", "Customer");
      }

      return RedirectToAction("Register", "Customer");
    }
  
    public ActionResult PasswordRecovery()
    {
      return View();
    }

    [HttpPost]
    public JsonResult PasswordRecovery(string email)
    {
      string recoveryPassword = DBHelper.getPassword(email);
      
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

    [HttpGet]
    public ActionResult Employee()
    {
      ViewBag.NhanViens = DBHelper.getEmployees();
      return View();
    }

    [HttpPost]
    public JsonResult Employee(string email)
    {
      DBHelper.deleteEmployee(email);
      return Json(new { message = "success" });
    }

    [HttpPost]
    public JsonResult UpdateEmployee(NhanVien nv)
    {
      DBHelper.updateEmployee(nv);
      NhanVien loginNv = (NhanVien)Session["NhanVien"];

      if (nv.Email == loginNv.Email)
      {
        Session["NhanVien"] = nv;
        return Json(new { loginNv = nv });
      }

      return Json(new { message = "successs" });
    }
  }
}