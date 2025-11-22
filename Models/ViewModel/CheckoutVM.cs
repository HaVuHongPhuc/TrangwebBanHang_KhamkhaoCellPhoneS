using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PagedList.Mvc;
using PagedList;
namespace TrangwebCellPhoneS.Models.ViewModel
{
    public class CheckoutVM
    {
        public List<CartItem> CartItems { get; set; }
        public int CustomerID { get; set; }

        [Display(Name = "Ngày đặt hàng")]
        public System.DateTime OrderDate { get; set; }

        [Display(Name = "Tổng giá trị")]
        public decimal TotalAmount { get; set; }

        [Display(Name = "Trạng thái thanh toán")]
        public string PaymentStatus { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn phương thức thanh toán")]
        public string PaymentMethod { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn phương thức giao hàng")]
        public string ShippingMethod { get; set; }

        [Display(Name = "Địa chỉ giao hàng")]
        public string ShippingAddress { get; set; }

        public string Username { get; set; }
        // Các thông tin người nhận hàng
        public string ConsigneeName { get; set; } // Họ tên ng nhận hàng
        public string ConsigneePhone { get; set; } // Số điện thoại ng nhận hàng
        public string ConsigneeEmail { get; set; } // Email ng nhận hàng
        public string AddressDetail { get; set; } // Số nhà, tên đường ng nhận hàng

        public string City { get; set; } // Tỉnh/Thành
        public string District { get; set; } // Quận/Huyện

        //Các thuộc tính khác của đơn hàng
        public List<OrderDetail> OrderDetails { get; set; }
    }
}