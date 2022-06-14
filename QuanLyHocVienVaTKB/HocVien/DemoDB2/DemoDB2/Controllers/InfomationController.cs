using DemoDB2.Models;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoDB2.Controllers
{
    public class InfomationController : Controller
    {
        HOCVIENVATKBEntities db = new HOCVIENVATKBEntities();
        // GET: Infomation
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "puRabLaGEv8EPLzZVxYM1yOOLQykx4PaxFOWwewN",
            BasePath = "https://quanlyhocvien-5fac6-default-rtdb.asia-southeast1.firebasedatabase.app/"
        };

        IFirebaseClient client;
        public static int maHv;
        public static string HoTen;
        public ActionResult Index()
        {
            if(Session["TENDANGNHAP"] == null)
            {
                return RedirectToAction("Index", "Admin");
            }
            client = new FireSharp.FirebaseClient(config);
            List<HOCVIEN> list = new List<HOCVIEN>();
            HOCVIEN data;
            if (client != null)
            {
                FirebaseResponse res = client.Get("QuanLyHocVienVaTKB");
                JObject job = JObject.Parse(res.Body.ToString());
                JArray jsonNode = (JArray)job["HocVien"];
                dynamic nodes = jsonNode;
                foreach(var node in nodes)
                {
                    string userName = Session["TENDANGNHAP"].ToString();
                    string user = (string)node["UserName"];
                    if (user.Equals(userName))
                    {
                        data = new HOCVIEN();
                        data.HocVienID = (int)node["HocVienID"];
                        data.DiaChi = (string)node["DiaChi"];
                        data.Email = (string)node["Email"];
                        data.GioiTinh = (string)node["GioiTinh"];
                        data.HoTen = (string)node["HoTen"];
                        string s = node["NgaySinh"];
                        if (!s.Contains(":"))
                        {
                            DateTime dateTime = DateTime.ParseExact(s, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            data.NgaySinh = dateTime;
                        }
                        else
                        {
                            data.NgaySinh = node["NgaySinh"];
                        }
                        maHv = data.HocVienID;
                        data.Password = node["Password"];
                        data.SoDienThoai = (string)node["SoDienThoai"];
                        data.UserName = (string)node["UserName"];
                        
                        list.Add(data);
                    }

                }

                
            }
            return View(list);
        }
        public void GetKhoaHoc()
        {
            client = new FireSharp.FirebaseClient(config);
            if(client != null)
            {
                FirebaseResponse res = client.Get("QuanLyHocVienVaTKB");
                JObject job = JObject.Parse(res.Body.ToString());
                JArray jsonNode = (JArray)job["KhoaHoc"];
                dynamic nodes = jsonNode;
                KHOAHOC kHOAHOC;
                foreach(var node in nodes)
                {
                    if(node != null )
                    {
                        kHOAHOC = new KHOAHOC();
                        try
                        {
                            kHOAHOC.KhoaHocID = node["KhoaHocID"];
                            kHOAHOC.TenKhoaHoc = node["TenKhoaHoc"];
                            kHOAHOC.Soluongsinhvien = node["Soluongsinhvien"];
                            var check = db.KHOAHOCs.Where(s => s.TenKhoaHoc == kHOAHOC.TenKhoaHoc).FirstOrDefault();
                            if (check == null)
                            {
                                db.KHOAHOCs.Add(kHOAHOC);
                                db.SaveChanges();
                            }

                        }
                        catch { }

                    }
                }
            }
            
        }
        public void GetHocVien()
        {
            client = new FireSharp.FirebaseClient(config);
            HOCVIEN data;
            if (client != null)
            {
                FirebaseResponse res = client.Get("QuanLyHocVienVaTKB");
                JObject job = JObject.Parse(res.Body.ToString());
                JArray jsonNode = (JArray)job["HocVien"];
                dynamic nodes = jsonNode;
                foreach (var node in nodes)
                {
                    if(node != null)
                    {
                        data = new HOCVIEN();
                        data.HocVienID = (int)node["HocVienID"];
                        data.DiaChi = (string)node["DiaChi"];
                        data.Email = (string)node["Email"];
                        data.GioiTinh = (string)node["GioiTinh"];
                        data.HoTen = (string)node["HoTen"];
                        string s = node["NgaySinh"];
                        if (!s.Contains(":"))
                        {
                            DateTime dateTime = DateTime.ParseExact(s, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            data.NgaySinh = dateTime;
                        }
                        else
                        {
                            data.NgaySinh = node["NgaySinh"];
                        }
                        data.Password = node["Password"];
                        data.SoDienThoai = (string)node["SoDienThoai"];
                        data.UserName = (string)node["UserName"];
                        var check = db.HOCVIENs.Where(s2 => s2.HoTen == data.HoTen && s2.UserName == data.UserName).FirstOrDefault();
                        if (check == null)
                        {
                            db.HOCVIENs.Add(data);
                            db.SaveChanges();
                        }
                    }
                    

                }
            }
        }
        public void GetThoiKhoaBieu()
        {
            client = new FireSharp.FirebaseClient(config);
            if (client != null)
            {
                FirebaseResponse res = client.Get("QuanLyHocVienVaTKB");
                JObject job = JObject.Parse(res.Body.ToString());
                JArray jsonNode = (JArray)job["TKB"];
                dynamic nodes = jsonNode;
                THOIKHOABIEU data;
                if (nodes != null)
                {
                    foreach (dynamic node in nodes)
                    {
                        if (node != null)
                        {
                            data = new THOIKHOABIEU();
                            data.ThoiKhoaBieuID = (int)node["ThoiKhoaBieuID"];
                            data.ChiTietID = (int)node["ChiTietID"];
                            data.HocVienID = (int)node["HocVienID"];
                            data.NgayBatDau = (DateTime)node["NgayBatDau"];
                            string s = node["NgayKetThuc"];
                            if (!s.Contains(":"))
                            {
                                DateTime dateTime = DateTime.ParseExact(s, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                data.NgayKetThuc = dateTime;
                            }
                            else
                            {
                                data.NgayKetThuc = node["NgayKetThuc"];
                            }
                            data.HocKy = (int)node["HocKy"];
                            bool kt = true;
                            var listmasach = db.THOIKHOABIEUx.ToList();
                            foreach (var item in listmasach)
                            {
                                if (item.HocVienID == data.HocVienID)
                                {
                                    if (item.ChiTietID == data.ChiTietID)
                                    {
                                        kt = false;
                                    }
                                }
                            }
                            if (kt == true)
                            {
                                db.THOIKHOABIEUx.Add(data);
                                db.SaveChanges();
                            }
                        }
                    }
                }
            }
        }
        public ActionResult BangDiem()
        {
            if (Session["TENDANGNHAP"] == null)
            {
                return RedirectToAction("Index", "Admin");
            }
            List<BANGDIEM> list = new List<BANGDIEM>();
            client = new FireSharp.FirebaseClient(config);
            if (client != null)
            {
                FirebaseResponse res = client.Get("QuanLyHocVienVaTKB");
                JObject job = JObject.Parse(res.Body.ToString());
                JArray jsonNode = (JArray)job["BangDiem"];
                dynamic nodes = jsonNode;
                BANGDIEM data;
                foreach (dynamic node in nodes)
                {
                    if (node != null)
                    {
                        int mahocvien = (int)node["HocVienID"];
                        if(mahocvien == maHv)
                        {
                            data = new BANGDIEM();
                            GetKhoaHoc();
                            GetHocVien();
                            data.HocVienID = (int)node["HocVienID"];
                            data.BangDiemID = (int)node["BangDiemID"];
                            data.ChiTietID = (int)node["ChiTietID"];
                            data.Diem1 = (decimal)node["Diem1"];
                            data.Diem2 = (decimal)node["Diem2"];
                            data.Diem3 = (decimal)node["Diem3"];
                            data.TongDiem = (byte)node["TongDiem"];
                            data.XepLoai = (string)node["XepLoai"];
                            data.LanThi = (byte)node["LanThi"];
                            data.HOCVIEN = db.HOCVIENs.Find(data.HocVienID);
                            data.CHITIETKHOAHOC = db.CHITIETKHOAHOCs.Find(data.ChiTietID);
                            list.Add(data);
                        }
                        
                    }
                }
            }
            return View(list);
        }

        int? id;
        public ActionResult ThoiKhoaBieu()
        {
            if (Session["TENDANGNHAP"] == null)
            {
                return RedirectToAction("Index", "Admin");
            }
            List<THOIKHOABIEU> list = new List<THOIKHOABIEU>();
            client = new FireSharp.FirebaseClient(config);
            if (client != null)
            {
                FirebaseResponse res = client.Get("QuanLyHocVienVaTKB");
                JObject job = JObject.Parse(res.Body.ToString());
                JArray jsonNode = (JArray)job["TKB"];
                dynamic nodes = jsonNode;
                THOIKHOABIEU data;
                if (nodes != null)
                {
                    foreach (dynamic node in nodes)
                    {
                        if (node != null)
                        {
                            int mahocvien = (int)node["HocVienID"];
                            if (mahocvien == maHv)
                            {
                                data = new THOIKHOABIEU();
                                data.ThoiKhoaBieuID = (int)node["ThoiKhoaBieuID"];
                                data.ChiTietID = (int)node["ChiTietID"];
                                data.HocVienID = (int)node["HocVienID"];
                                data.NgayBatDau = (DateTime)node["NgayBatDau"];
                                string s = node["NgayKetThuc"];
                                if (!s.Contains(":"))
                                {
                                    DateTime dateTime = DateTime.ParseExact(s, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    data.NgayKetThuc = dateTime;
                                }
                                else
                                {
                                    data.NgayKetThuc = node["NgayKetThuc"];
                                }
                                data.HocKy = (int)node["HocKy"];
                                data.CHITIETKHOAHOC = db.CHITIETKHOAHOCs.Find(data.ChiTietID);
                                data.HOCVIEN = db.HOCVIENs.Find(data.HocVienID);
                                list.Add(data);
                            }
                            
                        }
                    }
                }
            }
            return View(list);
        }
    }
}