using DemoDB2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoDB2.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        DBRaoVatEntities db = new DBRaoVatEntities();
        //public ActionResult Index()
        //{
        //    return View(db.CATEGORies.ToList());
        //}
        public ActionResult Create()
        {
            if (Session["TENDANGNHAP"] == null)
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult Create(CATEGORY x)
        {
            try
            {
                db.CATEGORies.Add(x);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return Content("Error Create New");
            }
        }
        public ActionResult Details(int id)
        {
            return View(db.CATEGORies.Where(s => s.MALOAI == id).FirstOrDefault());
        }
        public ActionResult Edit(int id)
        {
            return View(db.CATEGORies.Where(s => s.MALOAI == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult Edit(int id, CATEGORY x)
        {
            db.Entry(x).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            return View(db.CATEGORies.Where(s => s.MALOAI == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult Delete(int id, CATEGORY x)
        {
            try
            {
                x = db.CATEGORies.Where(s => s.MALOAI == id).FirstOrDefault();
                db.CATEGORies.Remove(x);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return Content("This data is using in other table, Error Delete!");
            }
        }
        public ActionResult Index(string _name)
        {
            if(Session["TENDANGNHAP"] == null)
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                if (_name == null)
                    return View(db.CATEGORies.ToList());
                else
                    return View(db.CATEGORies.Where(s => s.TENLOAI.Contains(_name)).ToList());
            }
        }
    }
}