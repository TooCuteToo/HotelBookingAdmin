using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HotelBookingAdmin.Models;

namespace HotelBookingAdmin.Helpers
{
  public class DBHelper
  {
    public static List<int> getStatistics()
    {
      using (QuanLyKhachSanDataContext db = new QuanLyKhachSanDataContext())
      {
        List<int> statistics = new List<int>()
        {
          db.NhanViens.Count(),
          db.KhachHangs.Count(),
          db.Phongs.Count(phong => phong.tinhTrang == "empty"),
          db.Phongs.Count(phong => phong.tinhTrang == "occupied")
        };

        return statistics;
      }
    }

    public static int countTotalRoom()
    {
      using (QuanLyKhachSanDataContext db = new QuanLyKhachSanDataContext())
      {
        return db.Phongs.Count();
      }
    }

    public static List<Phong> getPhongs()
    {
      using (QuanLyKhachSanDataContext db = new QuanLyKhachSanDataContext())
      {
        db.DeferredLoadingEnabled = false;
        return db.Phongs.ToList();
      }
    }

    public static List<double> getRatePercent()
    {
      using (QuanLyKhachSanDataContext db = new QuanLyKhachSanDataContext())
      {
        double badRate = (double)db.Phongs.Count(item => item.danhGia < 3) / db.Phongs.Count() * 100;
        double averageRate = (double)db.Phongs.Count(item => item.danhGia == 3) / db.Phongs.Count() * 100;
        double goodRate = (double)db.Phongs.Count(item => item.danhGia > 3) / db.Phongs.Count() * 100;

        double badRatePercent = Math.Round(badRate);
        double averagePercent = Math.Round(averageRate);
        double goodPercent = Math.Round(goodRate);

        List<double> percentList = new List<double>()
        {
          badRatePercent,
          averagePercent,
          goodPercent,
        };

        return percentList;
      }
    }

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

        if (nv != null) {
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
        } catch
        {
          return new NhanVien();
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

    public static void deleteEmployee(string email)
    {
      using (QuanLyKhachSanDataContext db = new QuanLyKhachSanDataContext())
      {
        NhanVien nv = db.NhanViens.FirstOrDefault(item => item.Email == email);
        db.NhanViens.DeleteOnSubmit(nv);
        db.SubmitChanges();
      }
    }

    public static void updateEmployee(NhanVien updatedNv)
    {
      using (QuanLyKhachSanDataContext db = new QuanLyKhachSanDataContext())
      {
        NhanVien nv = db.NhanViens.FirstOrDefault(item => item.Email == updatedNv.Email);

        nv.TenNV = updatedNv.TenNV;
        nv.MaCV = updatedNv.MaCV;

        nv.GioiTinh = updatedNv.GioiTinh;
        nv.NgaySinh = updatedNv.NgaySinh;
        nv.Email = updatedNv.Email;

        nv.SDT_NV = updatedNv.SDT_NV;
        nv.DiaChi_NV = updatedNv.DiaChi_NV;
        nv.Password = updatedNv.Password;

        db.SubmitChanges();
      }
    }

    public static List<Phong> updateRoom(Phong updatePhong)
    {
      using (QuanLyKhachSanDataContext db = new QuanLyKhachSanDataContext())
      {
        db.DeferredLoadingEnabled = false;
        Phong phong = db.Phongs.FirstOrDefault(item => item.tenPhong == updatePhong.tenPhong);
        HoaDon hd = db.HoaDons.FirstOrDefault(item => item.tenPhong == phong.tenPhong);

        phong.tinhTrang = updatePhong.tinhTrang;
        phong.maLoai = updatePhong.maLoai;
        phong.giaPhong = updatePhong.giaPhong;
        phong.giamGia = updatePhong.giamGia;

        if (hd != null)
        {
          double totalDays = hd.ngayTra.Value.Subtract(hd.ngayDat.Value).TotalDays;

          if (phong.giamGia != null)
          {
            hd.tienThanhToan = (phong.giaPhong - phong.giamGia) * (decimal)totalDays;
          } else
          {
            hd.tienThanhToan = phong.giaPhong * (decimal)totalDays;
          }
        }

        db.SubmitChanges();
        return db.Phongs.ToList();
      }
    }

    public static void deleteHoaDon(string tenPhong)
    {
      using (QuanLyKhachSanDataContext db = new QuanLyKhachSanDataContext())
      {
        HoaDon hd = db.HoaDons.FirstOrDefault(item => item.tenPhong == tenPhong);

        db.HoaDons.DeleteOnSubmit(hd);
        db.SubmitChanges();
      }
    }

    public static List<HoaDon> getHoaDons()
    {
      using (QuanLyKhachSanDataContext db = new QuanLyKhachSanDataContext())
      {
        db.DeferredLoadingEnabled = false;
        return db.HoaDons.ToList();
      }
    }

    public static List<KhachHang> getKhachHangs()
    {
      using (QuanLyKhachSanDataContext db = new QuanLyKhachSanDataContext())
      {
        return db.KhachHangs.ToList();
      }
    }

    public static HoaDon updateHoaDon(HoaDon updateHD)
    {
      using (QuanLyKhachSanDataContext db = new QuanLyKhachSanDataContext())
      {
        Phong phong = db.Phongs.FirstOrDefault(item => item.tenPhong == updateHD.tenPhong);
        HoaDon hd = db.HoaDons.FirstOrDefault(item => item.maHD == updateHD.maHD);

        hd.maHD = updateHD.maHD;
        hd.email = updateHD.email;
        hd.tenPhong = updateHD.tenPhong;
        hd.ngayDat = updateHD.ngayDat;
        hd.ngayTra = updateHD.ngayTra;

        double totalDays = hd.ngayTra.Value.Subtract(hd.ngayDat.Value).TotalDays;

        if (phong.giamGia != null)
        {
          hd.tienThanhToan = (phong.giaPhong - phong.giamGia) * (decimal) totalDays;
          db.SubmitChanges();

          return new HoaDon {
            maHD = hd.maHD,
            email = hd.email,
            tenPhong = hd.tenPhong,
            ngayDat = hd.ngayDat,
            ngayTra = hd.ngayTra,
            tienThanhToan = hd.tienThanhToan
          };
        }

        hd.tienThanhToan = phong.giaPhong * (decimal) totalDays;
        db.SubmitChanges();

        return new HoaDon
        {
          maHD = hd.maHD,
          email = hd.email,
          tenPhong = hd.tenPhong,
          ngayDat = hd.ngayDat,
          ngayTra = hd.ngayTra,
          tienThanhToan = hd.tienThanhToan
        };
      }
    }

    public static void deleteOrder(int maHD)
    {
      using (QuanLyKhachSanDataContext db = new QuanLyKhachSanDataContext())
      {
        HoaDon hd = db.HoaDons.FirstOrDefault(item => item.maHD == maHD);
        Phong phong = db.Phongs.FirstOrDefault(item => item.tenPhong == hd.tenPhong);
        phong.tinhTrang = "empty";
        db.HoaDons.DeleteOnSubmit(hd);
        db.SubmitChanges();
      }
    }

  }
}