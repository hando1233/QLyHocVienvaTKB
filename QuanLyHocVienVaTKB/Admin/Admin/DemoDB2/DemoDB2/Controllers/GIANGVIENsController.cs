using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DemoDB2.Models;

namespace DemoDB2.Controllers
{
    public class GIANGVIENsController : Controller
    {
        private HOCVIENVATKBEntities db = new HOCVIENVATKBEntities();

        // GET: GIANGVIENs
        public ActionResult Index()
        {
            return View(db.GIANGVIENs.ToList());
        }

        // GET: GIANGVIENs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GIANGVIEN gIANGVIEN = db.GIANGVIENs.Find(id);
            if (gIANGVIEN == null)
            {
                return HttpNotFound();
            }
            return View(gIANGVIEN);
        }

        // GET: GIANGVIENs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GIANGVIENs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GiangVienID,TenGiangVien,SoDienThoai,HocVi,Email,Matkhau")] GIANGVIEN gIANGVIEN)
        {
            if (ModelState.IsValid)
            {
                var check = db.GIANGVIENs.Where(s => s.TenGiangVien == gIANGVIEN.TenGiangVien && s.SoDienThoai == gIANGVIEN.SoDienThoai).FirstOrDefault();
                if (check == null)
                {
                    var list = db.GIANGVIENs.ToList();
                    gIANGVIEN.GiangVienID = list.Count;
                    db.GIANGVIENs.Add(gIANGVIEN);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(gIANGVIEN);
        }

        // GET: GIANGVIENs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GIANGVIEN gIANGVIEN = db.GIANGVIENs.Find(id);
            if (gIANGVIEN == null)
            {
                return HttpNotFound();
            }
            return View(gIANGVIEN);
        }

        // POST: GIANGVIENs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GiangVienID,TenGiangVien,SoDienThoai,HocVi,Email,Matkhau")] GIANGVIEN gIANGVIEN)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gIANGVIEN).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(gIANGVIEN);
        }

        // GET: GIANGVIENs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GIANGVIEN gIANGVIEN = db.GIANGVIENs.Find(id);
            if (gIANGVIEN == null)
            {
                return HttpNotFound();
            }
            return View(gIANGVIEN);
        }

        // POST: GIANGVIENs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GIANGVIEN gIANGVIEN = db.GIANGVIENs.Find(id);
            db.GIANGVIENs.Remove(gIANGVIEN);
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
