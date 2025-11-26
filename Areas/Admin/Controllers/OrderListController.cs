using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TrangwebCellPhoneS.Models;

namespace TrangwebCellPhoneS.Areas.Admin.Controllers
{
    public class OrderListController : Controller
    {
        private TrangwebCellPhoneSEntities db = new TrangwebCellPhoneSEntities();

        // 1. INDEX: Danh sách đơn hàng
        public ActionResult Index(bool? printSuccess)
        {
            // Sắp xếp đơn mới nhất lên đầu
            if (printSuccess == true)
            {
                TempData["SuccessMessage"] = "In hóa đơn thành công!";
            }
            var orders = db.Orders.OrderByDescending(o => o.OrderDate).ToList();
            return View(orders);
        }

        // 2. DETAILS: Chi tiết đơn hàng (Hóa đơn)
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var order = db.Orders.Find(id);
            if (order == null) return HttpNotFound();

            return View(order);
        }
    }
}