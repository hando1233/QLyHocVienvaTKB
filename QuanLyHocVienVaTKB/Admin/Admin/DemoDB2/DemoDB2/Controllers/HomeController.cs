using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoDB2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
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

        public ActionResult About()
        {
            if (Session["TENDANGNHAP"] == null)
            {
                return RedirectToAction("Index", "Admin");
            }
            else 
            { 
            ViewBag.Message = "Your application description page.";

            return View();
            }
        }

        public ActionResult Contact()
        {
            if (Session["TENDANGNHAP"] == null)
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                ViewBag.Message = "Your contact page.";

                return View();
            }
        }
    }
}