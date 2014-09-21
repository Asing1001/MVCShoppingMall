using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WecareMVC.Models
{
    [Bind(Exclude = "AlbumId")]
    public class Album
    {
        [ScaffoldColumn(false)]
        public int AlbumId { get; set; }
        [DisplayName("商品分類ID")]
        public int GenreId { get; set; }
        [DisplayName("製造商ID")]
        public int ArtistId { get; set; }
        [Required(ErrorMessage = "商品名稱 不得為空！")]
        [StringLength(160)]
        [DisplayName("商品名稱")]
        public string Title { get; set; }
        [Required(ErrorMessage = "單價 不得為空！")]
        [Range(0.01, 1000.00,
            ErrorMessage = "價格必須在 0.01 到 1000.00 之間")]
        [DisplayName("單價")]
        public decimal Price { get; set; }
        [DisplayName("圖片")]
        [StringLength(1024)]
        public string AlbumArtUrl { get; set; }
        public virtual Genre Genre { get; set; }
        public virtual Artist Artist { get; set; }
        public virtual List<OrderDetail> OrderDetails { get; set; }  
        //讓album可以連到orderdetail獲取數量，以呈現熱賣的商品
    }
}