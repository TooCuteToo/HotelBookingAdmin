using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HotelBookingAdmin.Models;

namespace HotelBookingAdmin.Helpers
{
  public class DBCustomer
  {
    #region Customer

    public static List<KhachHang> getCustomers()
    {
      using (QuanLyKhachSanDataContext db = new QuanLyKhachSanDataContext())
      {
        return db.KhachHangs.ToList();
      }
    }

    public static KhachHang createCustomer(KhachHang newCustomer)
    {
      using (QuanLyKhachSanDataContext db = new QuanLyKhachSanDataContext())
      {
        try
        {
          db.DeferredLoadingEnabled = false;
          db.KhachHangs.InsertOnSubmit(newCustomer);
          db.SubmitChanges();
          return newCustomer;
        }
        catch (Exception error)
        {
          return new KhachHang() { tenKH = null };
        }
      }
    }

    public static KhachHang UpdateCustomer(KhachHang updatedKH)
    {
      using (QuanLyKhachSanDataContext db = new QuanLyKhachSanDataContext())
      {
        try
        {
          db.DeferredLoadingEnabled = false;
          KhachHang kh = db.KhachHangs.FirstOrDefault(item => item.maKH == updatedKH.maKH);
          List<HoaDon> listHD = db.HoaDons.Where(item => item.maKH == updatedKH.maKH).ToList();

          foreach (HoaDon hd in listHD)
          {
            hd.email = updatedKH.email;
          }

          kh.email = updatedKH.email;
          kh.tenKH = updatedKH.tenKH;
          kh.gioiTinh = updatedKH.gioiTinh;
          kh.ngaySinh = updatedKH.ngaySinh;
          kh.diaChi = updatedKH.diaChi;
          kh.pass = updatedKH.pass;

          db.SubmitChanges();
          return kh;
        }
        catch (Exception error)
        {
          return new KhachHang() { tenKH = null };
        }
      }
    }

    public static string deleteCustomer(KhachHang deletedKH)
    {
      using (QuanLyKhachSanDataContext db = new QuanLyKhachSanDataContext())
      {
        try
        {
          KhachHang kh = db.KhachHangs.FirstOrDefault(item => item.maKH == deletedKH.maKH);
          db.KhachHangs.DeleteOnSubmit(kh);
          db.SubmitChanges();
          return "Success";
        }
        catch (Exception error)
        {
          return null;
        }
      }
    }

    #endregion
  }
}