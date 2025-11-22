using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrangwebCellPhoneS.Models;
using TrangwebCellPhoneS.Models.ViewModel;

namespace TrangwebCellPhoneS.Controllers
{
    public class OrderController : Controller
    {
         private TrangwebCellPhoneSEntities db = new TrangwebCellPhoneSEntities();
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }

        //GET: Order/Checkout
        [Authorize]
        public ActionResult Checkout()
        {
            var cart = Session["Cart"] as Cart;
            if (cart == null || !cart.Items.Any()) return RedirectToAction("Index", "Home");

             //xác thực người dùng đã đăng nhập chưa, neeus chưa thì chuyển hướng tới trang đăng nhập
            var user = db.Users.SingleOrDefault(u => u.Username == User.Identity.Name );
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Lấy thông tin giao hàng từ Session ra để hiển thị lại
            var shippingInfo = Session["ShippingInfo"] as CheckoutVM;
            if (shippingInfo == null) return RedirectToAction("Shipping"); // Nếu chưa nhập thì bắt quay lại

            // Gán thêm thông tin giỏ hàng để hiển thị tổng tiền
            shippingInfo.CartItems = cart.Items.ToList();
            shippingInfo.TotalAmount = cart.TotalValue();
            var customer = db.Customers.SingleOrDefault(c => c.Username == user.Username);
            if (customer == null) { return RedirectToAction("Login", "Account"); }
            int customerId = Convert.ToInt32(Session["CustomerID"]); //xem danh sách sản phẩm
            var model = new CheckoutVM //Tạo dữ liệu hiển thị cho Checkout
            {
                CartItems = cart.Items.ToList(), // Lấy danh sách sản phẩm trong giỏ hàng
                TotalAmount = cart.Items.Sum(item => item.TotalPrice), //Tổng giá trị của các mặt hàng trong giỏ
                OrderDate = DateTime.Now,//Mặc định lấy bằng thời điểm đặt hàng
                ShippingAddress = customer.CustomerAddress, //Lấy địa chỉ mặc định từ bảng Customer
                Username = customer.Username //Lấy tên đăng nhập từ bảng Customer
            };
            return View(shippingInfo);
        }

        //POST: Order/Checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Checkout(CheckoutVM model)
        {
            if (ModelState.IsValid)
            {
                var cart = Session["Cart"] as Cart;
                var shippingInfo = Session["ShippingInfo"] as CheckoutVM; // Lấy lại địa chỉ đã lưu
                if (cart == null) { return RedirectToAction("Index", "Home"); }

                //Nếu người dùng chưa đăng nhập thì sẽ điều hướng tới trang Login
                var user = db.Users.SingleOrDefault(u => u.Username == User.Identity.Name);
                if (user == null) { return RedirectToAction("Login", "Account"); }

                //Nếu khách hàng không khớp với tên đăng nhập, sẽ điều hướng tới trang login
                var customer = db.Customers.SingleOrDefault(c => c.Username == user.Username);
                if (customer == null) { return RedirectToAction("Login", "Account"); }

                //Nếu người dùng chọn thanh toán = paypal, sẽ điều hướng tới trang PaymentWithPaypal
                if (model.PaymentMethod == "Paypal")
                {
                    return RedirectToAction("PaymentWithPaypal", "Paypal", model);
                }

                //Thiết lập paymentstatus dựa trên paymentmethod
                string paymentStatus = "Chưa thanh toán";
                switch (model.PaymentMethod)
                {
                    case "Tiền mặt": paymentStatus = "Thanh toán tiền mặt"; break;
                    case "Paypal": paymentStatus = "Thanh toán paypal"; break;
                    case "Mua trước trả sau": paymentStatus = "Trả góp"; break;
                    default: paymentStatus = "Chưa thanh toán"; break;
                }

                // Tạo đơn hàng
                var order = new Order
                    {
                        CustomerID = customer.CustomerID,
                        OrderDate = DateTime.Now,
                        TotalAmount = cart.TotalValue(),
                        PaymentStatus = "Chưa thanh toán",
                        PaymentMethod = model.PaymentMethod, // Lấy phương thức thanh toán người dùng chọn
                        DeliveryMethod = "Tiêu chuẩn",

                        // Lấy thông tin từ bước Shipping
                        ShippingAddress = $"{shippingInfo.ShippingAddress}, {shippingInfo.District}, {shippingInfo.City}",
                        Consignee = shippingInfo.ConsigneeName,
                        ConsigneePhone = shippingInfo.ConsigneePhone,
                        ConsigneeEmail = shippingInfo.ConsigneeEmail,

                        OrderDetails = cart.Items.Select(i => new OrderDetail
                        {
                            ProductID = i.ProductID,
                            Quantity = i.Quantity,
                            UnitPrice = i.UnitPrice,
                            TotalPrice = i.TotalPrice
                        }).ToList()
                    };
                    //Lưu đơn hàng vào CSDL
                    db.Orders.Add(order);
                    db.SaveChanges();
                    //Xóa giỏ hàng sau khi đặt hàng thành công
                    Session["Cart"] = null;
                    //Điều hướng tới trang xác nhận đơn hàng
                    // Xóa giỏ và session tạm
                    Session["ShippingInfo"] = null;

                    return RedirectToAction("OrderSuccess", new { id = order.OrderID });
                }
           
            return View(model);
        }

        public ActionResult OrderSuccess(int? id) 
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var order = db.Orders
                          .Include("OrderDetails.Product")
                          .SingleOrDefault(o => o.OrderID == id);

            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        [Authorize] 
        public ActionResult OrderHistory()
        {
            // 1. Lấy username của người dùng đang đăng nhập
            var tenDangNhap = User.Identity.Name;

            // 2. Tìm CustomerID từ username
            var customer = db.Customers.SingleOrDefault(c => c.Username == tenDangNhap);

            // 3. Kiểm tra xem customer có tồn tại không
            if (customer == null)
            {
                // Nếu không tìm thấy thông tin khách hàng, có thể đưa về trang chủ
                return RedirectToAction("Index", "Home");
            }

            // 4. Lấy tất cả đơn hàng của khách hàng này
            // Sắp xếp theo ngày mới nhất lên trên
            var orders = db.Orders
                           .Where(o => o.CustomerID == customer.CustomerID)
                           .OrderByDescending(o => o.OrderDate)
                           .ToList(); // Lấy danh sách

            // 5. Gửi danh sách đơn hàng này tới View
            return View(orders);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Shipping()
        {
            // Kiểm tra giỏ hàng
            var cart = Session["Cart"] as Cart;
            if (cart == null || !cart.Items.Any()) return RedirectToAction("Index", "Home");

            // Lấy thông tin user điền sẵn (nếu có)
            var user = db.Users.SingleOrDefault(u => u.Username == User.Identity.Name);
            var customer = db.Customers.SingleOrDefault(c => c.Username == user.Username);

            var model = new CheckoutVM
            {
                ConsigneeName = customer?.CustomerName,
                ConsigneePhone = customer?.CustomerPhone,
                ConsigneeEmail = customer?.CustomerEmail,
                AddressDetail = customer?.CustomerAddress
            };

            return View(model); // Trả về View nhập địa chỉ
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Shipping(CheckoutVM model)
        {
            ModelState.Remove("PaymentMethod");
            ModelState.Remove("ShippingMethod");
            if (ModelState.IsValid)
            {
                // Tạm lưu thông tin giao hàng vào Session (Chưa lưu vào DB vội)
                Session["ShippingInfo"] = model;
                return RedirectToAction("Checkout"); // Chuyển sang bước thanh toán
            }
            return View(model);
        }
    }
}