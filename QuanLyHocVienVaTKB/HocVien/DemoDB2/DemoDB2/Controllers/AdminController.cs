using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DemoDB2.Models;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json.Linq;

namespace DemoDB2.Controllers
{
    public class AdminController : Controller
    {
        HOCVIENVATKBEntities db = new HOCVIENVATKBEntities();
        // GET: Admin
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "puRabLaGEv8EPLzZVxYM1yOOLQykx4PaxFOWwewN",
            BasePath = "https://quanlyhocvien-5fac6-default-rtdb.asia-southeast1.firebasedatabase.app/"
        };

        IFirebaseClient client;
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginAcc(FormCollection frc)
        {
            client = new FireSharp.FirebaseClient(config);
            var _adname = frc["admin_name"];
            var _pass = frc["admin_pass"];
            FirebaseResponse res = client.Get("QuanLyHocVienVaTKB");
            JObject job = JObject.Parse(res.Body.ToString());
            JArray jsonNode = (JArray)job["HocVien"];
            dynamic nodes = jsonNode;
            foreach (var node in nodes)
            {
                string user = (string)node["UserName"];
                if (user.Equals(_adname))
                {
                    string pass = (string)node["Password"];
                    if (pass.Equals(_pass))
                    {
                        db.Configuration.ValidateOnSaveEnabled = false;
                        Session["TENDANGNHAP"] = _adname;
                        Session["MATKHAU"] = _pass;
                        return RedirectToAction("Index", "Home");
                    }
                }

            }
            return RedirectToAction("Index", "Infomation");
        }
        public ActionResult LogOutUser()
        {
            Session.Abandon();
            return RedirectToAction("Index");
        }
    }
}