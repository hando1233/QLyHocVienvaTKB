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
    public class HOCVIENsController : Controller
    {
        private HOCVIENVATKBEntities db = new HOCVIENVATKBEntities();

        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "puRabLaGEv8EPLzZVxYM1yOOLQykx4PaxFOWwewN",
            BasePath = "https://quanlyhocvien-5fac6-default-rtdb.asia-southeast1.firebasedatabase.app/"
        };

        IFirebaseClient client;
        // GET: HOCVIENs
        public ActionResult Index()
        {
            client = new FireSharp.FirebaseClient(config);
            if (client != null)
            {
                FirebaseResponse res = client.Get("QuanLyHocVienVaTKB");
                JObject job = JObject.Parse(res.Body.ToString());
                JArray jsonNode = (JArray)job["HocVien"];
                dynamic nodes = jsonNode;
                HOCVIEN data;
                foreach (dynamic node in nodes)
                {
                    if (node != null)
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
                        bool kt = true;
                        var listmasach = db.HOCVIENs.ToList();
                        foreach (var item in listmasach)
                        {
                            if (item.UserName.Equals(data.UserName)) kt = false;
                        }
                        if (kt == true)
                        {
                            db.HOCVIENs.Add(data);
                            db.SaveChanges();
                        }
                    }
                }
            }
            return View(db.HOCVIENs.ToList());
        }

        // GET: HOCVIENs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HOCVIEN hOCVIEN = db.HOCVIENs.Find(id);
            if (hOCVIEN == null)
            {
                return HttpNotFound();
            }
            return View(hOCVIEN);
        }

        // GET: HOCVIENs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HOCVIENs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "HocVienID,HoTen,DiaChi,SoDienThoai,NgaySinh,Email,GioiTinh,UserName,Password")] HOCVIEN hOCVIEN)
        {
            client = new FireSharp.FirebaseClient(config);
            if (ModelState.IsValid)
            {
                bool kt = true;
                var listmasach = db.HOCVIENs.ToList();
                foreach (var item in listmasach)
                {
                    if (item.UserName.Equals(hOCVIEN.UserName)) kt = false;
                }
                if (kt == true)
                {
                    FirebaseResponse res = client.Get("QuanLyHocVienVaTKB");
                    JObject job = JObject.Parse(res.Body.ToString());
                    JArray jsonNode = (JArray)job["HocVien"];
                    dynamic nodes = jsonNode;
                    int count = nodes.Count;
                    hOCVIEN.HocVienID = count;
                    SetResponse res2 = client.Set("QuanLyHocVienVaTKB/HocVien/" + count, hOCVIEN);

                    return RedirectToAction("Index");
                }
            }

            //string tendangnhap = Session["TENDANGNHAP"].ToString();
            //var check = db.HOCVIENs.Where(s => s.UserName == tendangnhap).FirstOrDefault();
            //int maHv = check.HocVienID;
            //HOCVIEN hOCVIEN1 = db.HOCVIENs.Find(maHv);
            //var check2 = db.THOIKHOABIEUx.Where(s => s.HocVienID == maHv).FirstOrDefault();
            //THOIKHOABIEU tkb = db.THOIKHOABIEUx.Find(check2.ThoiKhoaBieuID);


            return View(hOCVIEN);
        }

        // GET: HOCVIENs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HOCVIEN hOCVIEN = db.HOCVIENs.Find(id);
            if (hOCVIEN == null)
            {
                return HttpNotFound();
            }
            return View(hOCVIEN);
        }

        // POST: HOCVIENs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "HocVienID,HoTen,DiaChi,SoDienThoai,NgaySinh,Email,GioiTinh,UserName,Password")] HOCVIEN hOCVIEN)
        {
            client = new FireSharp.FirebaseClient(config);
            if (ModelState.IsValid)
            {
                int maHv = hOCVIEN.HocVienID;
                maHv = maHv - 1;
                FirebaseResponse res = client.Update("QuanLyHocVienVaTKB/HocVien/" + maHv, hOCVIEN);
                //db.Entry(hOCVIEN).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hOCVIEN);
        }

        // GET: HOCVIENs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HOCVIEN hOCVIEN = db.HOCVIENs.Find(id);
            if (hOCVIEN == null)
            {
                return HttpNotFound();
            }
            return View(hOCVIEN);
        }

        // POST: HOCVIENs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            client = new FireSharp.FirebaseClient(config);
            HOCVIEN hOCVIEN = db.HOCVIENs.Find(id);
            db.HOCVIENs.Remove(hOCVIEN);
            db.SaveChanges();
            id = id - 1;
            FirebaseResponse res = client.Delete("QuanLyHocVienVaTKB/HocVien/" + id);
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
