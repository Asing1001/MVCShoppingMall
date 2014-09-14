using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WecareMVC.Models
{
    public class Artist
    {
        [DisplayName("製造商ID")]
        public int ArtistId { get; set; }
        [DisplayName("製造商名稱")]
        public string Name { get; set; }
    }
}