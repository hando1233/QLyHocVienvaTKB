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
    public class KHOAHOCsController : Controller
    {
        private HOCVIENVATKBEntities db = new HOCVIENVATKBEntities();

        // GET: KHOAHOCs
        public ActionResult Index()
        {
            return View(db.KHOAHOCs.ToList());
        }

        // GET: KHOAHOCs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHOAHOC kHOAHOC = db.KHOAHOCs.Find(id);
            if (kHOAHOC == null)
            {
                return HttpNotFound();
            }
            return View(kHOAHOC);
        }

        // GET: KHOAHOCs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: KHOAHOCs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "KhoaHocID,TenKhoaHoc,Soluongsinhvien")] KHOAHOC kHOAHOC)
        {
            if (ModelState.IsValid)
            {
                var check = db.KHOAHOCs.Where(s => s.KhoaHocID == kHOAHOC.KhoaHocID).FirstOrDefault();
                if (check == null)
                {
                    var list = db.KHOAHOCs.ToList();
                    kHOAHOC.KhoaHocID = list.Count;
                    db.KHOAHOCs.Add(kHOAHOC);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(kHOAHOC);
        }

        // GET: KHOAHOCs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHOAHOC kHOAHOC = db.KHOAHOCs.Find(id);
            if (kHOAHOC == null)
            {
                return HttpNotFound();
            }
            return View(kHOAHOC);
        }

        // POST: KHOAHOCs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "KhoaHocID,TenKhoaHoc,Soluongsinhvien")] KHOAHOC kHOAHOC)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kHOAHOC).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(kHOAHOC);
        }

        // GET: KHOAHOCs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHOAHOC kHOAHOC = db.KHOAHOCs.Find(id);
            if (kHOAHOC == null)
            {
                return HttpNotFound();
            }
            return View(kHOAHOC);
        }

        // POST: KHOAHOCs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            KHOAHOC kHOAHOC = db.KHOAHOCs.Find(id);
            db.KHOAHOCs.Remove(kHOAHOC);
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
