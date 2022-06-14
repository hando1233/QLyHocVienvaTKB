using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
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
    public class THOIKHOABIEUxController : Controller
    {
        private HOCVIENVATKBEntities db = new HOCVIENVATKBEntities();

        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "puRabLaGEv8EPLzZVxYM1yOOLQykx4PaxFOWwewN",
            BasePath = "https://quanlyhocvien-5fac6-default-rtdb.asia-southeast1.firebasedatabase.app/"
        };

        IFirebaseClient client;
        public ActionResult Index()
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
            return View(db.THOIKHOABIEUx.ToList());
        }

        // GET: THOIKHOABIEUx/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            THOIKHOABIEU tHOIKHOABIEU = db.THOIKHOABIEUx.Find(id);
            if (tHOIKHOABIEU == null)
            {
                return HttpNotFound();
            }
            return View(tHOIKHOABIEU);
        }

        // GET: THOIKHOABIEUx/Create
        public ActionResult Create()
        {
            ViewBag.ChiTietID = new SelectList(db.CHITIETKHOAHOCs, "ChiTietID", "KhoaHocID");
            ViewBag.HocVienID = new SelectList(db.HOCVIENs, "HocVienID", "HoTen");
            return View();
        }

        // POST: THOIKHOABIEUx/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ThoiKhoaBieuID,ChiTietID,HocVienID,NgayBatDau,NgayKetThuc,HocKy")] THOIKHOABIEU tHOIKHOABIEU)
        {
            client = new FireSharp.FirebaseClient(config);
            if (ModelState.IsValid)
            {
                var check = db.THOIKHOABIEUx.Where(s => s.HocVienID == tHOIKHOABIEU.HocVienID && s.ChiTietID == tHOIKHOABIEU.ChiTietID).FirstOrDefault();
                if (check == null)
                {
                    FirebaseResponse res = client.Get("QuanLyHocVienVaTKB");
                    JObject job = JObject.Parse(res.Body.ToString());
                    JArray jsonNode = (JArray)job["TKB"];
                    dynamic nodes = jsonNode;
                    int count = nodes.Count;
                    tHOIKHOABIEU.ThoiKhoaBieuID = count;
                   
                    SetResponse res2 = client.Set("QuanLyHocVienVaTKB/TKB/" + count, tHOIKHOABIEU);
                    return RedirectToAction("Index");
                }
            }

            ViewBag.ChiTietID = new SelectList(db.CHITIETKHOAHOCs, "ChiTietID", "KhoaHocID", tHOIKHOABIEU.ChiTietID);
            ViewBag.HocVienID = new SelectList(db.HOCVIENs, "HocVienID", "HoTen", tHOIKHOABIEU.HocVienID);
            return View(tHOIKHOABIEU);
        }

        // GET: THOIKHOABIEUx/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            THOIKHOABIEU tHOIKHOABIEU = db.THOIKHOABIEUx.Find(id);
            if (tHOIKHOABIEU == null)
            {
                return HttpNotFound();
            }
            ViewBag.ChiTietID = new SelectList(db.CHITIETKHOAHOCs, "ChiTietID", "KhoaHocID", tHOIKHOABIEU.ChiTietID);
            ViewBag.HocVienID = new SelectList(db.HOCVIENs, "HocVienID", "HoTen", tHOIKHOABIEU.HocVienID);
            return View(tHOIKHOABIEU);
        }

        // POST: THOIKHOABIEUx/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ThoiKhoaBieuID,ChiTietID,HocVienID,NgayBatDau,NgayKetThuc,HocKy")] THOIKHOABIEU tHOIKHOABIEU)
        {
            client = new FireSharp.FirebaseClient(config);
            if (ModelState.IsValid)
            {
                db.Entry(tHOIKHOABIEU).State = EntityState.Modified;
                db.SaveChanges();
                FirebaseResponse res = client.Update("QuanLyHocVienVaTKB/TKB/" + tHOIKHOABIEU.ThoiKhoaBieuID, tHOIKHOABIEU);
                return RedirectToAction("Index");
            }
            ViewBag.ChiTietID = new SelectList(db.CHITIETKHOAHOCs, "ChiTietID", "PhongHocId", tHOIKHOABIEU.ChiTietID);
            ViewBag.HocVienID = new SelectList(db.HOCVIENs, "HocVienID", "HoTen", tHOIKHOABIEU.HocVienID);
            return View(tHOIKHOABIEU);
        }

        // GET: THOIKHOABIEUx/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            THOIKHOABIEU tHOIKHOABIEU = db.THOIKHOABIEUx.Find(id);
            if (tHOIKHOABIEU == null)
            {
                return HttpNotFound();
            }
            return View(tHOIKHOABIEU);
        }

        // POST: THOIKHOABIEUx/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            client = new FireSharp.FirebaseClient(config);
            THOIKHOABIEU tHOIKHOABIEU = db.THOIKHOABIEUx.Find(id);
            db.THOIKHOABIEUx.Remove(tHOIKHOABIEU);
            db.SaveChanges();
            FirebaseResponse res = client.Delete("QuanLyHocVienVaTKB/TKB/" + id);
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