using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrangwebCellPhoneS.Models;
using TrangwebCellPhoneS.Models.ViewModel;
using PagedList.Mvc;

namespace TrangwebCellPhoneS.Models.ViewModel
{
    public class HomeProductVM
    {
        //Tiêu chí để search theo tên, mô tả sp
        //hoặc loại sản phẩm
        public string SearchTerm { get; set; }

        //Các thuộc tính hỗ trợ phân trang
        public int PageNumber { get; set; } //Trang hiện tại
        public int PageSize { get; set; } //Số sản phẩm mỗi trang

        //Danh sách sản phẩm nổi bật
        public List<Product> FeaturedProducts { get; set; }

        //Danh sách sản phẩm mới nhất đã phân trang
        public PagedList.IPagedList<Product> NewProducts { get; set; }

        public List<Product> MobileProducts { get; set; } // Danh sách điện thoại
        public List<Product> LaptopProducts { get; set; } // Danh sách Laptop
        public List<Product> AudioProducts { get; set; }  // Danh sách Tai nghe/Loa
    }
}