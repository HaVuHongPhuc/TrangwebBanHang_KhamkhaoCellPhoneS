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
        public string CustomerName { get; set; }

        [Display(Name = "Nickname")]
        public string Username { get; set; } // Dùng Username làm Nickname

        [Display(Name = "Số điện thoại")]
        public string CustomerPhone { get; set; }

        [Display(Name = "Địa chỉ email")]
        public string CustomerEmail { get; set; }

        [Display(Name = "Địa chỉ nhận hàng")]
        public string CustomerAddress { get; set; }

        [Display(Name = "Giới tính")]
        public string CustomerGender { get; set; } // Nam/ Nữ/ Khác

        [Display(Name = "Ngày sinh")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? CustomerBirthday { get; set; }
    }
}