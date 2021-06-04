using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HotelBookingAdmin.Models;

namespace HotelBookingAdmin.Helpers
{
  public class DBHelper
  {
    #region Get/Calculate statistics

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

    #endregion
  }
}