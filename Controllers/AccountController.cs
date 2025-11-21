using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
using TrangwebCellPhoneS.Models;
using TrangwebCellPhoneS.Models.ViewModel;
using System.Runtime.Remoting.Messaging;

namespace TrangwebCellPhoneS.Controllers
{
    public class AccountController : Controller
    {
        private TrangwebCellPhoneSEntities db = new TrangwebCellPhoneSEntities();
        // GET: Account
        public ActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                //kiểm tra xem tên đăng nhập đã tồn tại chưa
                var existingUser = db.Users.SingleOrDefault(u => u.Username == model.Username);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Username", "Tên đăng nhập này đã tồn tại!");
                    return View(model);
                }

                //nếu chưa tồn tại thì tạo bản ghi thông tin tài khoản trong bảng User
                var user = new User
                {
                    Username = model.Username,
                    Password = model.Password, // Lưu ý: Nên mã hóa mật khẩu trước khi lưu
                    UserRole = "customer"
                };
                db.Users.Add(user);

                //và tạo bản ghi thông tin khách hàng trong bảng Customer
                var customer = new Customer
                {
                    CustomerName = model.CustomerName,
                    CustomerEmail = model.CustomerEmail,
                    CustomerPhone = model.CustomerPhone,
                    CustomerAddress = model.CustomerAddress,
                    Username = model.Username,
                };
                db.Customers.Add(customer);

                //lưu thông tin tài khoản và thông tin khách hàng vào CSDL
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }
        // GET: Account/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.SingleOrDefault(u => u.Username == model.Username
                                                      && u.Password == model.Password
                                                      && u.UserRole == "customer");
                if (user != null)
                {
                    // === CODE MỚI BẮT ĐẦU TỪ ĐÂY ===

                    // 1. Lấy thông tin khách hàng từ username
                    var customer = db.Customers.SingleOrDefault(c => c.Username == user.Username);

                    // 2. Lưu thông tin vào Session
                    Session["Username"] = user.Username; // Vẫn giữ để xác thực
                    Session["UserRole"] = user.UserRole;
                    if (customer != null)
                    {
                        // Lưu TÊN KHÁCH HÀNG để chào
                        Session["CustomerName"] = customer.CustomerName;
                    }
                    else
                    {
                        // Trường hợp dự phòng nếu bảng Customer không có
                        Session["CustomerName"] = user.Username;
                    }

                    //lưu thông tin xác thực người dùng vào cookie (vẫn dùng Username)
                    FormsAuthentication.SetAuthCookie(user.Username, false);

                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }
        // GET: Account/Logout
        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut(); // Hủy cookie xác thực
            return RedirectToAction("Index", "Home"); // Quay về trang chủ
        }
    }
}