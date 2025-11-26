using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TrangwebCellPhoneS.Models;
using TrangwebCellPhoneS.Models.ViewModel;

namespace TrangwebCellPhoneS.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        private TrangwebCellPhoneSEntities db = new TrangwebCellPhoneSEntities();
        // GET: Admin/User
        public ActionResult Index()
        {
            // Lọc: Chỉ lấy UserRole là 'Admin'
            var admins = db.Users.Where(u => u.UserRole == "Admin").ToList();
            return View(admins);
        }

        // 2. CREATE (GET)
        public ActionResult Create()
        {
            return View();
        }
        // 2. CREATE (POST) - Có kiểm tra Validation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AdminUserVM model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem tên đăng nhập đã tồn tại chưa
                if (db.Users.Any(u => u.Username == model.Username))
                {
                    ModelState.AddModelError("Username", "Tên đăng nhập này đã tồn tại.");
                    return View(model);
                }

                // Chuyển từ ViewModel sang Model Database
                var user = new User
                {
                    Username = model.Username,
                    Password = model.Password, // Lưu ý: Thực tế nên mã hóa mật khẩu ở đây
                    UserRole = "Admin" // Cố định quyền là Admin
                };

                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        // 3. EDIT (GET)
        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id)) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var user = db.Users.Find(id);
            if (user == null) return HttpNotFound();

            // Đưa dữ liệu cũ lên form để sửa
            var model = new AdminUserVM
            {
                Username = user.Username,
                Password = user.Password,
                ConfirmPassword = user.Password, // Gán tạm để không bị lỗi validation
                UserRole = user.UserRole
            };
            return View(model);
        }

        // 3. EDIT (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AdminUserVM model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Find(model.Username);
                if (user != null)
                {
                    user.Password = model.Password;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        // 4. DELETE
        public ActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var user = db.Users.Find(id);
            if (user != null)
            {
                db.Users.Remove(user);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}