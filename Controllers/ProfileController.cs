using System.Linq;
using System.Web.Mvc;
using TrangwebCellPhoneS.Models;
using TrangwebCellPhoneS.Models.ViewModel;

namespace TrangwebCellPhoneS.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private TrangwebCellPhoneSEntities db = new TrangwebCellPhoneSEntities();

        // GET: Profile/Index
        public ActionResult Index()
        {
            var username = User.Identity.Name;
            var customer = db.Customers.SingleOrDefault(c => c.Username == username);

            if (customer == null) return RedirectToAction("Index", "Home");

            var model = new ProfileVM
            {
                // 1. Các thông tin cũ
                Username = customer.Username,
                CustomerName = customer.CustomerName,
                CustomerEmail = customer.CustomerEmail,
                CustomerAddress = customer.CustomerAddress,


                CustomerPhone = customer.CustomerPhone,
                CustomerGender = customer.Gender,    
            };

            return View(model);
        }

        // POST: Profile/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(ProfileVM model)
        {
            if (ModelState.IsValid)
            {
                var username = User.Identity.Name;
                var customer = db.Customers.SingleOrDefault(c => c.Username == username);

                if (customer != null)
                {
                    // 1. Cập nhật thông tin từ ViewModel vào Database Entity
                    customer.CustomerName = model.CustomerName;
                    customer.CustomerPhone = model.CustomerPhone;
                    customer.CustomerEmail = model.CustomerEmail;
                    customer.CustomerAddress = model.CustomerAddress;
                    customer.Gender = model.CustomerGender;    // VM là CustomerGender -> DB là Gender

                    // 2. Lưu xuống SQL
                    db.SaveChanges();

                    // 3. Cập nhật lại Session để Header đổi tên ngay lập tức
                    Session["CustomerName"] = customer.CustomerName;

                    TempData["SuccessMessage"] = "Cập nhật thông tin thành công!";
                }
            }
            return View("Index", model);
        }
    }
}