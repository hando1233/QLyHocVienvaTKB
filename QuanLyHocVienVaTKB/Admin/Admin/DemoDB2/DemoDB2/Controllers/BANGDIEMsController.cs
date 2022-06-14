using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DemoDB2.Models;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json.Linq;

namespace DemoDB2.Controllers
{
    public class BANGDIEMsController : Controller
    {
        private HOCVIENVATKBEntities db = new HOCVIENVATKBEntities();

        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "puRabLaGEv8EPLzZVxYM1yOOLQykx4PaxFOWwewN",
            BasePath = "https://quanlyhocvien-5fac6-default-rtdb.asia-southeast1.firebasedatabase.app/"
        };

        IFirebaseClient client;
        // GET: BANGDIEMs
        public ActionResult Index()
        {
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

                        data = new BANGDIEM();
                        data.HocVienID = (int)node["HocVienID"];
                        data.BangDiemID = (int)node["BangDiemID"];
                        data.ChiTietID = (int)node["ChiTietID"];
                        data.Diem1 = (decimal)node["Diem1"];
                        data.Diem2 = (decimal)node["Diem2"];
                        data.Diem3 = (decimal)node["Diem3"];
                        data.TongDiem = (byte)node["TongDiem"];
                        data.XepLoai = (string)node["XepLoai"];
                        data.LanThi = (byte)node["LanThi"];
                        var check = db.BANGDIEMs.Where(s => s.ChiTietID == data.ChiTietID && s.HocVienID == data.HocVienID).FirstOrDefault();
                        if (check == null)
                        {
                            db.BANGDIEMs.Add(data);
                            db.SaveChanges();
                        }
                    }
                }
            }

            return View(db.BANGDIEMs.ToList());
            //var bANGDIEMs = db.BANGDIEMs.Include(b => b.CHITIETKHOAHOC.KHOAHOC).Include(b => b.HOCVIEN);
            //return View(bANGDIEMs.ToList());
        }

        // GET: BANGDIEMs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BANGDIEM bANGDIEM = db.BANGDIEMs.Find(id);
            if (bANGDIEM == null)
            {
                return HttpNotFound();
            }
            return View(bANGDIEM);
        }

        // GET: BANGDIEMs/Create
        public ActionResult Create()
        {
            ViewBag.ChiTietID = new SelectList(db.CHITIETKHOAHOCs, "ChiTietID", "KhoaHocID");
            ViewBag.HocVienID = new SelectList(db.HOCVIENs, "HocVienID", "HoTen");
            return View();
        }

        // POST: BANGDIEMs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BangDiemID,ChiTietID,HocVienID,HoTen,Diem1,Diem2,Diem3,TongDiem,XepLoai,LanThi")] BANGDIEM bANGDIEM)
        {
            client = new FireSharp.FirebaseClient(config);
            if (ModelState.IsValid)
            {
                double diem1 = (int)bANGDIEM.Diem1;
                double diem2 = (int)bANGDIEM.Diem2;
                double diem3 = (int)bANGDIEM.Diem3;
                double tongdiem = (diem1 + diem2 + diem3) / 3;
                bANGDIEM.TongDiem = (byte)tongdiem;
                if (tongdiem >= 8)
                {
                    bANGDIEM.XepLoai = "Gioi";
                }
                else if (tongdiem >= 6.5 && tongdiem < 8)
                {
                    bANGDIEM.XepLoai = "Kha";
                }
                else if (tongdiem >= 5 && tongdiem < 6.5)
                {
                    bANGDIEM.XepLoai = "Trung binh";
                }
                else if (tongdiem >= 3.5 && tongdiem < 5)
                {
                    bANGDIEM.XepLoai = "Yeu";
                }
                else
                {
                    bANGDIEM.XepLoai = "Khong du dieu kien";
                }
                //db.BANGDIEMs.Add(bANGDIEM);
                //db.SaveChanges();
                FirebaseResponse res = client.Get("QuanLyHocVienVaTKB");
                JObject job = JObject.Parse(res.Body.ToString());
                JArray jsonNode = (JArray)job["BangDiem"];
                dynamic nodes = jsonNode;
                int count = nodes.Count;
                bANGDIEM.ChiTietID = count;
                var check = db.BANGDIEMs.Where(s => s.ChiTietID == bANGDIEM.ChiTietID && s.HocVienID == bANGDIEM.HocVienID).FirstOrDefault();
                if (check == null)
                {
                    SetResponse res2 = client.Set("QuanLyHocVienVaTKB/BangDiem/" + count, bANGDIEM);
                    return RedirectToAction("Index");
                }
            }

            ViewBag.ChiTietID = new SelectList(db.CHITIETKHOAHOCs, "ChiTietID", "KhoaHocID", bANGDIEM.ChiTietID);
            ViewBag.HocVienID = new SelectList(db.HOCVIENs, "HocVienID", "HoTen", bANGDIEM.HocVienID);
            return View(bANGDIEM);
        }
        public ActionResult ThongKe ()
        {
            
                int countHocSinhGioi = 0;
                int countHocSinhKha = 0;
                int countHocSinhTrungBinh = 0;
                int countHocSinhYeu = 0;
                int countHocSinhKhongDuDieuKien = 0;
                int countRot = 0;
                var listbandiem = db.BANGDIEMs.ToList();
                foreach (var item in listbandiem)
                {
                    if (item.XepLoai == "Gioi")
                    {
                    countHocSinhGioi++;
                    }
                if (item.XepLoai == "Kha")
                {
                    countHocSinhKha++;
                }
                if (item.XepLoai == "Trung binh")
                {
                    countHocSinhTrungBinh++;
                }
                if (item.XepLoai == "Yeu")
                {
                     countHocSinhYeu++;
                }
                else
                    {
                    countHocSinhKhongDuDieuKien++;
                    }
                }
                ViewBag.SoHocSinhGioi = countHocSinhGioi.ToString();
                ViewBag.SoHocSinhKha = countHocSinhKha.ToString();
                ViewBag.SoHocSinhTB = countHocSinhTrungBinh.ToString();
            ViewBag.SoHocSinhYeu = countHocSinhYeu.ToString();
            ViewBag.SoHocSinhKhongDuDieuKien = countHocSinhKhongDuDieuKien.ToString();
            return View();
            }
            
        
        // GET: BANGDIEMs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BANGDIEM bANGDIEM = db.BANGDIEMs.Find(id);
            if (bANGDIEM == null)
            {
                return HttpNotFound();
            }
            ViewBag.ChiTietID = new SelectList(db.CHITIETKHOAHOCs, "ChiTietID", "KhoaHocID", bANGDIEM.ChiTietID);
            ViewBag.HocVienID = new SelectList(db.HOCVIENs, "HocVienID", "HoTen", bANGDIEM.HocVienID);
            return View(bANGDIEM);
        }

        // POST: BANGDIEMs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BangDiemID,ChiTietID,HocVienID,HoTen,Diem1,Diem2,Diem3,TongDiem,XepLoai,LanThi")] BANGDIEM bANGDIEM)
        {
            client = new FireSharp.FirebaseClient(config);
            if (ModelState.IsValid)
            {
                db.Entry(bANGDIEM).State = EntityState.Modified;
                db.SaveChanges();
                int bandiem = bANGDIEM.BangDiemID;
                FirebaseResponse res = client.Update("QuanLyHocVienVaTKB/BangDiem/" + bandiem, bANGDIEM);
                return RedirectToAction("Index");
            }
            ViewBag.ChiTietID = new SelectList(db.CHITIETKHOAHOCs, "ChiTietID", "KhoaHocID", bANGDIEM.ChiTietID);
            ViewBag.HocVienID = new SelectList(db.HOCVIENs, "HocVienID", "HoTen", bANGDIEM.HocVienID);
            return View(bANGDIEM);
        }

        // GET: BANGDIEMs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BANGDIEM bANGDIEM = db.BANGDIEMs.Find(id);
            if (bANGDIEM == null)
            {
                return HttpNotFound();
            }
            return View(bANGDIEM);
        }

        // POST: BANGDIEMs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BANGDIEM bANGDIEM = db.BANGDIEMs.Find(id);
            db.BANGDIEMs.Remove(bANGDIEM);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
