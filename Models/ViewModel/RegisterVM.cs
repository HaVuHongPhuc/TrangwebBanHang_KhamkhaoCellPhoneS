using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrangwebCellPhoneS.Models.ViewModel
{
    public class RegisterVM
    {
        [Required]
        [Display(Name = "Tên đăng nhập")]
        [StringLength(50, MinimumLength = 10 ,ErrorMessage ="Tên không hợp lệ vui lòng nhập lại. Tên đăng nhập tối thiểu 10 ký tự và tối đa 50 ký tự")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        [StringLength(255, MinimumLength = 10, ErrorMessage = "Mật khẩu quá yếu, vui lòng nhập lại. Tối thiếu 10 ký tự")]

        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu và xác nhận mật khẩu không khớp.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Họ tên")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "Tên không hợp lệ vui lòng nhập lại. Tên đăng nhập tối thiểu 10 ký tự và tối đa 50 ký tự")]
        public string CustomerName { get; set; }

        [Required]
        [Display(Name = "Số điện thoại")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(11, MinimumLength = 9, ErrorMessage = "SĐT không hợp lệ, vui lòng nhập lại")]
        public string CustomerPhone { get; set; }

        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress (ErrorMessage = "Email không hợp lệ")]
        public string CustomerEmail { get; set; }

        [Required]
        [Display(Name = "Địa chỉ")]
        public string CustomerAddress { get; set; }


        [Display(Name = "Giới tính")]
        public string CustomerGender { get; set; } // Nam/ Nữ/ Khác
    }
}