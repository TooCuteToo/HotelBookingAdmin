using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HotelBookingAdmin.Models;

namespace HotelBookingAdmin.Helpers
{
  public class DBOrder
  {
    #region Order

    public static void deleteHoaDon(int maHD)
    {
      using (QuanLyKhachSanDataContext db = new QuanLyKhachSanDataContext())
      {
        HoaDon hd = db.HoaDons.FirstOrDefault(item => item.maHD == maHD);

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

    public static HoaDon getHoaDon(HoaDon hd)
    {
      using (QuanLyKhachSanDataContext db = new QuanLyKhachSanDataContext())
      {
        db.DeferredLoadingEnabled = false;
        return db.HoaDons.FirstOrDefault(item => item.maHD == hd.maHD);
      }
    }

    public static HoaDon getHoaDon(int maHD)
    {
      using (QuanLyKhachSanDataContext db = new QuanLyKhachSanDataContext())
      {
        return db.HoaDons.FirstOrDefault(item => item.maHD == maHD);
      }
    }

    public static HoaDon updateHoaDon(HoaDon updateHD)
    {
      using (QuanLyKhachSanDataContext db = new QuanLyKhachSanDataContext())
      {
        Phong phong = db.Phongs.FirstOrDefault(item => item.tenPhong == updateHD.tenPhong);
        HoaDon hd = db.HoaDons.FirstOrDefault(item => item.maHD == updateHD.maHD);

        if (hd.tenPhong != updateHD.tenPhong)
        {
          db.Phongs.FirstOrDefault(item => item.tenPhong == hd.tenPhong).tinhTrang = "empty";
          Phong updatedPhong = db.Phongs.FirstOrDefault(item => item.tenPhong == updateHD.tenPhong);
          updatedPhong.tinhTrang = "occupied";
          hd.maPhong = updatedPhong.maPhong;
        }

        hd.maNV = updateHD.maNV;
        hd.email = updateHD.email;
        hd.tenPhong = updateHD.tenPhong;
        hd.ngayDat = updateHD.ngayDat;
        hd.ngayTra = updateHD.ngayTra;
        hd.tinhTrang = updateHD.tinhTrang;

        if (hd.tinhTrang == true)
        {
          db.Phongs.FirstOrDefault(item => item.tenPhong == updateHD.tenPhong).tinhTrang = "empty";
        }

        else if (hd.tinhTrang == false)
        {
          db.Phongs.FirstOrDefault(item => item.tenPhong == updateHD.tenPhong).tinhTrang = "occupied";
        }

        double totalDays = hd.ngayTra.Value.Subtract(hd.ngayDat.Value).TotalDays;

        if (phong.giamGia != null)
        {
          hd.tienThanhToan = (phong.giaPhong - phong.giamGia) * (decimal)totalDays;
          db.SubmitChanges();

          return new HoaDon
          {
            maHD = hd.maHD,
            maNV = hd.maNV,
            email = hd.email,
            tenPhong = hd.tenPhong,
            maPhong = hd.maPhong,
            tinhTrang = hd.tinhTrang,
            ngayDat = hd.ngayDat,
            ngayTra = hd.ngayTra,
            tienThanhToan = hd.tienThanhToan
          };
        }

        hd.tienThanhToan = phong.giaPhong * (decimal)totalDays;
        db.SubmitChanges();

        return new HoaDon
        {
          maHD = hd.maHD,
          maNV = hd.maNV,
          email = hd.email,
          tenPhong = hd.tenPhong,
          maPhong = hd.maPhong,
          tinhTrang = hd.tinhTrang,
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
        DBServices.removeServicesDetail(hd);
        db.HoaDons.DeleteOnSubmit(hd);
        db.SubmitChanges();
      }
    }

    public static HoaDon createHoaDon(HoaDon newHD)
    {
      using (QuanLyKhachSanDataContext db = new QuanLyKhachSanDataContext())
      {
        db.DeferredLoadingEnabled = false;
        Phong phong = db.Phongs.FirstOrDefault(item => item.tenPhong == newHD.tenPhong);
        phong.tinhTrang = "occupied";

        newHD.maPhong = phong.maPhong;
        newHD.maKH = db.KhachHangs.FirstOrDefault(item => item.email == newHD.email).maKH;

        double totalDays = newHD.ngayTra.Value.Subtract(newHD.ngayDat.Value).TotalDays;

        if (phong.giamGia != null)
        {
          newHD.tienThanhToan = (phong.giaPhong - phong.giamGia) * (decimal)totalDays;

          db.HoaDons.InsertOnSubmit(newHD);
          db.SubmitChanges();

          return newHD;
        }

        newHD.tienThanhToan = phong.giaPhong * (decimal)totalDays;

        db.HoaDons.InsertOnSubmit(newHD);
        db.SubmitChanges();

        return newHD;
      }
    }

    #endregion
  }
}