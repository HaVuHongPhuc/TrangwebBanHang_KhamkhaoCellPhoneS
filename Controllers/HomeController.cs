using PagedList;
using PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TrangwebCellPhoneS.Models;
using TrangwebCellPhoneS.Models.ViewModel;

namespace TrangwebCellPhoneS.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult CusInf()
        {
            return View();
        }
        private TrangwebCellPhoneSEntities db = new TrangwebCellPhoneSEntities();
        public ActionResult Index(string searchTerm, int? page)
        {
            var model = new HomeProductVM();
            var products = db.Products.AsQueryable();
            //Tìm kiếm sản phẩm dựa trên từ khóa
            if (!string.IsNullOrEmpty(searchTerm))
            {
                model.SearchTerm = searchTerm;
                products = products.Where(p => p.ProductName.Contains(searchTerm) ||
                                        p.ProductSpecifications.Contains(searchTerm) ||
                                        p.outstandingFeatures.Contains(searchTerm)||
                                        p.Category.CategoryName.Contains(searchTerm));
            }

            // 1. LẤY ĐIỆN THOẠI (Thay số 1 bằng ID thật của bạn)
            model.MobileProducts = products
                .Where(p => p.CategoryID == 3)
                .OrderByDescending(p => p.OrderDetails.Count)
                .Take(10).ToList();

            // 2. LẤY LAPTOP (Thay số 2 bằng ID thật của bạn)
            model.LaptopProducts = products
                .Where(p => p.CategoryID == 1)
                .OrderByDescending(p => p.OrderDetails.Count)
                .Take(10).ToList();

            // 3. LẤY ÂM THANH (Thay số 3 bằng ID thật của bạn)
            model.AudioProducts = products
                .Where(p => p.CategoryID == 2)
                .OrderByDescending(p => p.OrderDetails.Count)
                .Take(10).ToList();

            //Đoạn code liên quan tới phân trang
            //Lấy số trang hiện tại (mặc định là trang 1 nếu không có giá trị)
            int pageNumber = page ?? 1;
            int pageSize = 15; //Số sản phẩm mỗi trang

            //Láy top 6 sản phẩm bán chạy nhất
            model.FeaturedProducts = products
                                        .OrderByDescending(p => p.OrderDetails.Count())
                                        .Take(6)
                                        .ToList();

            //Lấy 5 sản phẩm bán ế nhất và phân trang
            model.NewProducts = products
                        .OrderBy(p => p.OrderDetails.Count())
                        .Take(6)
                        .ToPagedList(pageNumber, pageSize);
            return View(model);
        }

        // GET: Home/ProductDetails/5
        public ActionResult ProductDetails(int? id, int? quantity, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Product pro = db.Products.Find(id);
            if (pro == null)
            {
                return HttpNotFound();
            }

            // Lấy tất cả các sản phẩm cùng danh mục
            var products = db.Products
                .Where(p => p.CategoryID == pro.CategoryID && p.ProductID != pro.ProductID)
                .AsQueryable();

            ProductDetailsVM model = new ProductDetailsVM();

            // Đoạn code liên quan tới phân trang
            // Lấy số trang hiện tại (mặc định là trang 1 nếu không có giá trị)
            int pageNumber = page ?? 1;
            int pageSize = model.PageSize; // Số sản phẩm mỗi trang

            model.product = pro;
            model.RelatedProducts = products.OrderBy(p => p.ProductID).Take(8).ToPagedList(pageNumber, pageSize);
            model.TopProducts = products.OrderByDescending(p => p.OrderDetails.Count()).Take(8).ToPagedList(pageNumber, pageSize);

            if (quantity.HasValue)
            {
                model.quantity = quantity.Value;
            }

            return View(model);
        }

        public ActionResult _PVFeatureProduct()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult _PVNewProduct()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        // 1. Trang Điện thoại
        public ActionResult Dienthoai()
        {
            var products = db.Products
                .Where(p => p.Category.CategoryName.Contains("Điện thoại")) // Lọc theo tên danh mục
                .OrderByDescending(p => p.OrderDetails.Count) // Sắp xếp theo bán chạy (hoặc ID tùy bạn)
                .Take(10) // Chỉ lấy tối đa 10 sản phẩm
                .ToList();

            return View(products);
        }

        // 2. Trang Laptop
        public ActionResult Laptop()
        {
            var products = db.Products
                .Where(p => p.Category.CategoryName.Contains("Laptop"))
                .OrderByDescending(p => p.OrderDetails.Count)
                .Take(10)
                .ToList();

            return View(products);
        }

        // 3. Trang Âm thanh (Tai nghe)
        public ActionResult AmThanh()
        {
            var products = db.Products
                .Where(p => p.Category.CategoryName.Contains("Tai nghe") || p.Category.CategoryName.Contains("Tai nghe"))
                .OrderByDescending(p => p.OrderDetails.Count)
                .Take(10)
                .ToList();

            return View(products);
        }
    }
}