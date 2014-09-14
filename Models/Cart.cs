using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WecareMVC.Models
{
    public class Cart
    {
        [Key]
        public int RecordId { get; set; }
        public string CartId { get; set; }
        public int AlbumId { get; set; }

        [Required(AllowEmptyStrings = true, ErrorMessage = " ")]  //空白可以不顯示錯誤訊息 又可以不讓使用者輸空白
        [Range(0, 100, ErrorMessage = "數量必須為 0 ~ 100")]
        [DisplayName("數量")]
        public int Count { get; set; }
        public System.DateTime DateCreated { get; set; }
        public virtual Album Album { get; set; }
    }

    //public partial class Order
    //{
    //    public int OrderId { get; set; }
    //    public string Username { get; set; }
    //    public string FirstName { get; set; }
    //    public string LastName { get; set; }
    //    public string Address { get; set; }
    //    public string City { get; set; }
    //    public string State { get; set; }
    //    public string PostalCode { get; set; }
    //    public string Country { get; set; }
    //    public string Phone { get; set; }
    //    public string Email { get; set; }
    //    public decimal Total { get; set; }
    //    public System.DateTime OrderDate { get; set; }
    //    public List<OrderDetail> OrderDetails { get; set; }
    //}

    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int AlbumId { get; set; }
        [DisplayName("數量")]
        public int Quantity { get; set; }
        [DisplayName("單價")]
        public decimal UnitPrice { get; set; }
        public virtual Album Album { get; set; }
        public virtual Order Order { get; set; }
    }
}