using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WecareMVC.Models
{
    [Bind(Exclude = "OrderId")]
    public partial class Order
    {
        [ScaffoldColumn(false)]
        public int OrderId { get; set; }

        [ScaffoldColumn(false)]
        public System.DateTime OrderDate { get; set; }

        [ScaffoldColumn(false)]
        public string Username { get; set; }

        [Required(ErrorMessage = "名 為必填")]
        [DisplayName("名：")]
        [StringLength(160)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "姓 為必填")]
        [DisplayName("姓：")]
        [StringLength(160)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "地址 為必填")]
        [StringLength(70)]
        [DisplayName("地址：")]
        public string Address { get; set; }

        [Required(ErrorMessage = "城市 為必填")]
        [StringLength(40)]
        [DisplayName("城市：")]
        public string City { get; set; }

        //[Required(ErrorMessage = "*")]
        //[StringLength(40)]
        //[DisplayName("洲")]
        //public string State { get; set; }

        [Required(ErrorMessage = "郵遞區號 為必填")]
        [DisplayName("郵遞區號：")]
        [StringLength(10)]        
        public string PostalCode { get; set; }
        [Required(ErrorMessage = "國家 為必填")]
        [StringLength(40)]
        [DisplayName("國家：")]
        public string Country { get; set; }
        [Required(ErrorMessage = "聯絡電話 為必填")]
        [StringLength(24)]
        [DisplayName("連絡電話：")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "電子郵件 為必填")]
        [DisplayName("Email：")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",
            ErrorMessage = "不正確的郵件地址")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [ScaffoldColumn(false)]
        public decimal Total { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }
    }
}