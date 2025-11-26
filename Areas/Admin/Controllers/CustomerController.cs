using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TrangwebCellPhoneS.Models;

namespace TrangwebCellPhoneS.Areas.Admin.Controllers
{
    public class CustomerController : Controller
    {
        private TrangwebCellPhoneSEntities db = new TrangwebCellPhoneSEntities();
        
        // GET: Admin/Customers (Chỉ xem danh sách)
        public ActionResult Index()
        {
            return View(db.Customers.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Tìm khách hàng
            Customer customer = db.Customers.Find(id);

            if (customer == null)
            {
                return HttpNotFound();
            }

            // Trả về View, View này sẽ tự lấy danh sách Orders từ quan hệ khóa ngoại
            return View(customer);
        }
    }
}