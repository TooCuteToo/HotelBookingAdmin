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
        } catch (Exception e)
        {
          return new NhanVien() { TenNV = null  };
        }
      }
    }

    #endregion

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
        } catch (Exception error)
        {
          return new List<Phong>();
        }
      }
    }

    #endregion

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
          hd.tienThanhToan = (phong.giaPhong - phong.giamGia) * (decimal) totalDays;
          db.SubmitChanges();

          return new HoaDon {
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

        hd.tienThanhToan = phong.giaPhong * (decimal) totalDays;
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
        removeServicesDetail(hd);
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
        } catch (Exception error)
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
        } catch (Exception error)
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
        } catch (Exception error)
        {
          return null;
        }
      }
    }

    #endregion

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