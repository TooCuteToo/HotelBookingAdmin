using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HotelBookingAdmin.Models;

namespace HotelBookingAdmin.Helpers
{
  public class DBEmployee
  {
    #region Employee

    public static NhanVien getLoginEmployee(string email, string password)
    {
      using (QuanLyKhachSanDataContext db = new QuanLyKhachSanDataContext())
      {
        return db.NhanViens.FirstOrDefault(item => item.Email == email && item.Password == password);
      }
    }

    public static string getPassword(string email)
    {
      using (QuanLyKhachSanDataContext db = new QuanLyKhachSanDataContext())
      {
        NhanVien nv = db.NhanViens.FirstOrDefault(item => item.Email == email);

        if (nv != null)
        {
          return nv.Password;
        }

        return "";
      }
    }

    public static NhanVien createEmployee(string fullName, string phoneNumber, string dob, string email)
    {
      using (QuanLyKhachSanDataContext db = new QuanLyKhachSanDataContext())
      {
        try
        {
          NhanVien nv = new NhanVien()
          {
            TenNV = fullName,
            SDT_NV = phoneNumber,
            Email = email,
            NgaySinh = DateTime.Parse(dob),
            MaCV = "employee",
          };

          db.NhanViens.InsertOnSubmit(nv);
          db.SubmitChanges();

          return nv;
        }
        catch
        {
          return new NhanVien() { TenNV = null };
        }
      }
    }

    public static List<NhanVien> getEmployees()
    {
      using (QuanLyKhachSanDataContext db = new QuanLyKhachSanDataContext())
      {
        return db.NhanViens.ToList();
      }
    }

    public static string deleteEmployee(int maNV)
    {
      using (QuanLyKhachSanDataContext db = new QuanLyKhachSanDataContext())
      {
        try
        {
          NhanVien nv = db.NhanViens.FirstOrDefault(item => item.MaNV == maNV);
          db.NhanViens.DeleteOnSubmit(nv);
          db.SubmitChanges();

          return "success";
        }
        catch (Exception error)
        {
          return null;
        }
      }
    }

    public static NhanVien updateEmployee(NhanVien updatedNv)
    {
      using (QuanLyKhachSanDataContext db = new QuanLyKhachSanDataContext())
      {
        try
        {
          db.DeferredLoadingEnabled = false;
          NhanVien nv = db.NhanViens.FirstOrDefault(item => item.MaNV == updatedNv.MaNV);

          nv.TenNV = updatedNv.TenNV;
          nv.MaCV = updatedNv.MaCV;

          nv.GioiTinh = updatedNv.GioiTinh;
          nv.NgaySinh = updatedNv.NgaySinh;
          nv.Email = updatedNv.Email;

          nv.SDT_NV = updatedNv.SDT_NV;
          nv.DiaChi_NV = updatedNv.DiaChi_NV;
          nv.Password = updatedNv.Password;

          db.SubmitChanges();

          return nv;
        }
        catch (Exception e)
        {
          return new NhanVien() { TenNV = null };
        }
      }
    }

    #endregion
  }
}