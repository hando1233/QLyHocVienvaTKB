using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DemoDB2.Models;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json.Linq;

namespace DemoDB2.Controllers
{
    public class CHITIETKHOAHOCsController : Controller
    {
        private HOCVIENVATKBEntities db = new HOCVIENVATKBEntities();
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "puRabLaGEv8EPLzZVxYM1yOOLQykx4PaxFOWwewN",
            BasePath = "https://quanlyhocvien-5fac6-default-rtdb.asia-southeast1.firebasedatabase.app/"
        };
        private FirebaseClient client;

        // GET: CHITIETKHOAHOCs
        public ActionResult Index()
        {
            var cHITIETKHOAHOCs = db.CHITIETKHOAHOCs.Include(c => c.GIANGVIEN).Include(c => c.KHOAHOC).Include(c => c.PHONGHOC);
            return View(cHITIETKHOAHOCs.ToList());
        }

        // GET: CHITIETKHOAHOCs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CHITIETKHOAHOC cHITIETKHOAHOC = db.CHITIETKHOAHOCs.Find(id);
            if (cHITIETKHOAHOC == null)
            {
                return HttpNotFound();
            }
            return View(cHITIETKHOAHOC);
        }

        // GET: CHITIETKHOAHOCs/Create
        public ActionResult Create()
        {
            ViewBag.GiangVienID = new SelectList(db.GIANGVIENs, "GiangVienID", "TenGiangVien");
            ViewBag.KhoaHocID = new SelectList(db.KHOAHOCs, "KhoaHocID", "TenKhoaHoc");
            ViewBag.PhongHocId = new SelectList(db.PHONGHOCs, "PhongHocId", "TenPhongHoc");
            return View();
        }

        // POST: CHITIETKHOAHOCs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ChiTietID,GiangVienID,PhongHocId,KhoaHocID,NgayBatDauKhoaHoc,NgayKetThucKhoaHoc,SoTiet,TietBatDau")] CHITIETKHOAHOC cHITIETKHOAHOC)
        {
            client = new FireSharp.FirebaseClient(config);
            if (ModelState.IsValid)
            {
                bool kt = true;
                var listmasach = db.CHITIETKHOAHOCs.ToList();
                foreach (var item in listmasach)
                {
                    if (item.ChiTietID.Equals(cHITIETKHOAHOC.ChiTietID)) kt = false;
                }
                if (kt == true)
                {
                    FirebaseResponse res = client.Get("QuanLyHocVienVaTKB");
                    JObject job = JObject.Parse(res.Body.ToString());
                    JArray jsonNode = (JArray)job["ChiTietKhoaHoc"];
                    dynamic nodes = jsonNode;
                    int count = nodes.Count;
                   /* cHITIETKHOAHOC.ChiTietID = count;*/
                    SetResponse res2 = client.Set("QuanLyHocVienVaTKB/ChiTietKhoaHoc/" + count, cHITIETKHOAHOC);

                    return RedirectToAction("Index");
                }
            }
           
            ViewBag.GiangVienID = new SelectList(db.GIANGVIENs, "GiangVienID", "TenGiangVien", cHITIETKHOAHOC.GiangVienID);
            ViewBag.KhoaHocID = new SelectList(db.KHOAHOCs, "KhoaHocID", "TenKhoaHoc", cHITIETKHOAHOC.KhoaHocID);
            ViewBag.PhongHocId = new SelectList(db.PHONGHOCs, "PhongHocId", "TenPhongHoc", cHITIETKHOAHOC.PhongHocId);
            return View(cHITIETKHOAHOC);
        }

        // GET: CHITIETKHOAHOCs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CHITIETKHOAHOC cHITIETKHOAHOC = db.CHITIETKHOAHOCs.Find(id);
            if (cHITIETKHOAHOC == null)
            {
                return HttpNotFound();
            }
            ViewBag.GiangVienID = new SelectList(db.GIANGVIENs, "GiangVienID", "TenGiangVien", cHITIETKHOAHOC.GiangVienID);
            ViewBag.KhoaHocID = new SelectList(db.KHOAHOCs, "KhoaHocID", "TenKhoaHoc", cHITIETKHOAHOC.KhoaHocID);
            ViewBag.PhongHocId = new SelectList(db.PHONGHOCs, "PhongHocId", "LoaiPhongHoc", cHITIETKHOAHOC.PhongHocId);
            return View(cHITIETKHOAHOC);
        }

        // POST: CHITIETKHOAHOCs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ChiTietID,GiangVienID,PhongHocId,KhoaHocID,NgayBatDauKhoaHoc,NgayKetThucKhoaHoc,SoTiet,TietBatDau")] CHITIETKHOAHOC cHITIETKHOAHOC)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cHITIETKHOAHOC).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GiangVienID = new SelectList(db.GIANGVIENs, "GiangVienID", "TenGiangVien", cHITIETKHOAHOC.GiangVienID);
            ViewBag.KhoaHocID = new SelectList(db.KHOAHOCs, "KhoaHocID", "TenKhoaHoc", cHITIETKHOAHOC.KhoaHocID);
            ViewBag.PhongHocId = new SelectList(db.PHONGHOCs, "PhongHocId", "TenPhongHoc", cHITIETKHOAHOC.PhongHocId);
            return View(cHITIETKHOAHOC);
        }

        // GET: CHITIETKHOAHOCs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CHITIETKHOAHOC cHITIETKHOAHOC = db.CHITIETKHOAHOCs.Find(id);
            if (cHITIETKHOAHOC == null)
            {
                return HttpNotFound();
            }
            return View(cHITIETKHOAHOC);
        }

        // POST: CHITIETKHOAHOCs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CHITIETKHOAHOC cHITIETKHOAHOC = db.CHITIETKHOAHOCs.Find(id);
            db.CHITIETKHOAHOCs.Remove(cHITIETKHOAHOC);
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
