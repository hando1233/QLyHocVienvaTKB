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
    public class KHOAHOCsController : Controller
    {
        private HOCVIENVATKBEntities db = new HOCVIENVATKBEntities();

        //bắt buộc phải có
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "puRabLaGEv8EPLzZVxYM1yOOLQykx4PaxFOWwewN",
            BasePath = "https://quanlyhocvien-5fac6-default-rtdb.asia-southeast1.firebasedatabase.app/"
        };

        IFirebaseClient client;

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

            //bắc buộc phải có
            client = new FireSharp.FirebaseClient(config);

            if (ModelState.IsValid)
            {
                // check điều kiện trùng
                var check = db.KHOAHOCs.Where(s => s.TenKhoaHoc == kHOAHOC.TenKhoaHoc).FirstOrDefault();
                //Nếu không có thì tạo
                if (check == null)
                {
                    // lấy dữ liệu khóa học về
                    FirebaseResponse res = client.Get("QuanLyHocVienVaTKB");
                    JObject job = JObject.Parse(res.Body.ToString());
                    JArray jsonNode = (JArray)job["KhoaHoc"];
                    dynamic nodes = jsonNode;
                    //set id cho khóa học ( nếu để id tự tăng thì khỏi cần set
                    //kHOAHOC.KhoaHocID = nodes.Count;

                    db.KHOAHOCs.Add(kHOAHOC);
                    db.SaveChanges();
                    //lấy dữ liệu khóa học trên fb về đém có bao nhiêu cái 0-5 là 5 cái. Sau cái /Khoahoc/ là để đếm tới cái thứ mấy rồi ví dụ đã có 5 thì h là 6
                    // kHOAHOC là 1 class dữ liệu để post lên 
                    SetResponse res2 = client.Set("QuanLyHocVienVaTKB/KhoaHoc/" + nodes.Count, kHOAHOC);
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
