using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HotelBookingAdmin.Models;

namespace HotelBookingAdmin.Helpers
{
  public class DBServices
  {
    #region Services

    public static List<DichVu> getServices()
    {
      using (QuanLyKhachSanDataContext db = new QuanLyKhachSanDataContext())
      {
        return db.DichVus.ToList();
      }
    }

    public static List<ChiTietDichVu> getServiceDetailByOrder(int maHD)
    {
      using (QuanLyKhachSanDataContext db = new QuanLyKhachSanDataContext())
      {
        db.DeferredLoadingEnabled = false;
        return db.ChiTietDichVus.Where(item => item.maHD == maHD).ToList();
      }
    }

    public static void createServicesDetail(HoaDon hd, string[] services)
    {
      using (QuanLyKhachSanDataContext db = new QuanLyKhachSanDataContext())
      {
        if (services == null)
        {
          ChiTietDichVu ct = new ChiTietDichVu()
          {
            maHD = hd.maHD,
            maDichVu = 6,
          };

          db.ChiTietDichVus.InsertOnSubmit(ct);
          db.SubmitChanges();

          db.HoaDons.FirstOrDefault(item => item.maHD == hd.maHD).tienThanhToan += getTotalServicesMoney(hd);
          db.SubmitChanges();

          return;
        }

        List<ChiTietDichVu> listServices = new List<ChiTietDichVu>();

        foreach (string service in services)
        {
          ChiTietDichVu ct = new ChiTietDichVu()
          {
            maHD = hd.maHD,
            maDichVu = int.Parse(service),
          };

          listServices.Add(ct);

        }

        db.ChiTietDichVus.InsertAllOnSubmit(listServices);
        db.SubmitChanges();

        db.HoaDons.FirstOrDefault(item => item.maHD == hd.maHD).tienThanhToan += getTotalServicesMoney(hd);
        db.SubmitChanges();
      }
    }

    public static void removeServicesDetail(HoaDon hd)
    {
      using (QuanLyKhachSanDataContext db = new QuanLyKhachSanDataContext())
      {
        List<ChiTietDichVu> services = db.ChiTietDichVus.Where(item => item.maHD == hd.maHD).ToList();

        foreach (ChiTietDichVu service in services)
        {
          db.ChiTietDichVus.DeleteOnSubmit(service);
          db.SubmitChanges();
        }
      }
    }

    public static decimal getTotalServicesMoney(HoaDon hd)
    {
      using (QuanLyKhachSanDataContext db = new QuanLyKhachSanDataContext())
      {
        List<ChiTietDichVu> ctDichVus = db.ChiTietDichVus.Where(item => item.maHD == hd.maHD).ToList();

        decimal totalMoney = 0;

        foreach (ChiTietDichVu ct in ctDichVus)
        {
          totalMoney += Decimal.Parse(db.DichVus.FirstOrDefault(item => item.maDichVu == ct.maDichVu).giaTien.ToString());
        }

        return totalMoney;
      }
    }

    #endregion 
  }
}