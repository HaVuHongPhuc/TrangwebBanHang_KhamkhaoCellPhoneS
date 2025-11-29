using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrangwebCellPhoneS.Models.ViewModel
{
    public class ProfileVM
    {
        // Thông tin cá nhân
        [Display(Name = "Họ & Tên")]
        [StringLength(50, MinimumLength = 10, ErrorMessage ="Tên đăng nhập không hợp lệ vui lòng nhập lại")]
        public string CustomerName { get; set; }

        [Display(Name = "Nickname")]
        public string Username { get; set; } // Dùng Username làm Nickname

        [Display(Name = "Số điện thoại")]
        [StringLength(11, MinimumLength = 9, ErrorMessage = "Số điện thoại không hợp lệ vui lòng nhập lại")]
        public string CustomerPhone { get; set; }

        [Display(Name = "Địa chỉ email")]
        [EmailAddress (ErrorMessage ="Email không hợp lệ")]
        public string CustomerEmail { get; set; }

        [Display(Name = "Địa chỉ nhận hàng")]
        public string CustomerAddress { get; set; }

        [Display(Name = "Giới tính")]
        public string CustomerGender { get; set; } // Nam/ Nữ/ Khác
    }
}