using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrangwebCellPhoneS.Models;

namespace TrangwebCellPhoneS.Areas.Admin.Controllers
{
    public class KhohangController : Controller
    {
        private TrangwebCellPhoneSEntities db = new TrangwebCellPhoneSEntities();
        // GET: Admin/Khohang
        public ActionResult Index()
        {
            return View();
        }

    }
}