using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TrangwebCellPhoneS.Models;

namespace TrangwebCellPhoneS.Areas.Admin.Controllers
{
    public class CategoriesController : Controller
    {
        private TrangwebCellPhoneSEntities db = new TrangwebCellPhoneSEntities();

        // GET: Admin/Categories
        public ActionResult Index()
        {
            return View(db.Categories.ToList());
        }

        // GET: Admin/Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Admin/Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoryID,CategoryName")] Category category)
        {
            if (db.Categories.Any(c => c.CategoryName == category.CategoryName))
            {
                // Thêm lỗi vào ModelState. "CategoryName" là tên thuộc tính sẽ hiển thị lỗi bên dưới nó
                ModelState.AddModelError("CategoryName", "Danh mục này đã tồn tại.");
            }
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Admin/Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Admin/Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoryID,CategoryName")] Category category)
        {
            // 1. Kiểm tra xem danh mục này có đang chứa sản phẩm nào không
            bool coSanPham = db.Products.Any(p => p.CategoryID == category.CategoryID);

            if (coSanPham)
            {
                // Nếu có sản phẩm, KHÔNG cho sửa
                // Cách 1: Thêm lỗi vào ModelState để hiển thị tại trang Edit
                ModelState.AddModelError("", "Không thể sửa danh mục này vì đang có sản phẩm thuộc về nó.");
                return View(category);

                // Cách 2 (Nếu bạn muốn đá về trang Index và hiện thông báo):
                /*
                TempData["ErrorMessage"] = "Không thể sửa danh mục này vì đang có sản phẩm thuộc về nó.";
                return RedirectToAction("Index");
                */
            }
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Admin/Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Admin/Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);

            // 1. Kiểm tra xem có sản phẩm nào đang thuộc danh mục này không
            bool coSanPham = db.Products.Any(p => p.CategoryID == id);

            if (coSanPham)
            {
                // 2. Nếu có, báo lỗi và không xóa
                // Dùng TempData để chuyển thông báo lỗi sang trang Index hoặc Delete
                TempData["ErrorMessage"] = "Không thể xóa danh mục này vì đang có sản phẩm bên trong. Vui lòng xóa sản phẩm trước.";
                return RedirectToAction("Index");
            }

            // 3. Nếu không có sản phẩm, xóa bình thường
            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
