using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HotelBookingAdmin.Models;

namespace HotelBookingAdmin.Helpers
{
  public class DBRoom
  {
    #region Room

    public static List<Phong> getPhongs()
    {
      using (QuanLyKhachSanDataContext db = new QuanLyKhachSanDataContext())
      {
        db.DeferredLoadingEnabled = false;
        return db.Phongs.ToList();
      }
    }

    public static List<Phong> updateRoom(Phong updatePhong)
    {
      using (QuanLyKhachSanDataContext db = new QuanLyKhachSanDataContext())
      {
        try
        {
          db.DeferredLoadingEnabled = false;
          Phong phong = db.Phongs.FirstOrDefault(item => item.maPhong == updatePhong.maPhong);
          HoaDon hd = db.HoaDons.FirstOrDefault(item => item.maPhong == phong.maPhong);

          phong.tenPhong = updatePhong.tenPhong;
          phong.tinhTrang = updatePhong.tinhTrang;
          phong.maLoai = updatePhong.maLoai;
          phong.giaPhong = updatePhong.giaPhong;
          phong.giamGia = updatePhong.giamGia != 0 ? updatePhong.giamGia : null;

          if (hd != null)
          {
            double totalDays = hd.ngayTra.Value.Subtract(hd.ngayDat.Value).TotalDays;
            hd.tenPhong = phong.tenPhong;

            if (phong.giamGia != null)
            {
              hd.tienThanhToan = (phong.giaPhong - phong.giamGia) * (decimal)totalDays;
            }
            else
            {
              hd.tienThanhToan = phong.giaPhong * (decimal)totalDays;
            }
          }

          db.SubmitChanges();
          return db.Phongs.ToList();
        }
        catch (Exception error)
        {
          return new List<Phong>();
        }
      }
    }

    public static Phong getPhong(string tenPhong)
    {
      using (QuanLyKhachSanDataContext db = new QuanLyKhachSanDataContext())
      {
        db.DeferredLoadingEnabled = false;
        return db.Phongs.FirstOrDefault(item => item.tenPhong == tenPhong);
      }
    }

    public static LoaiPhong getLoaiPhong(string tenPhong)
    {
      using (QuanLyKhachSanDataContext db = new QuanLyKhachSanDataContext())
      {
        db.DeferredLoadingEnabled = false;
        Phong phong = getPhong(tenPhong);

        return db.LoaiPhongs.FirstOrDefault(item => item.maLoai == phong.maLoai);
      }
    }

    #endregion
  }
}